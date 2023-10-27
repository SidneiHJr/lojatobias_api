
using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Extensions;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;
using LojaTobias.Infra.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LojaTobias.Identidade.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly UserManager<AspnetUserExtension> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AspnetUserExtension> _signInManager;

        public AuthController(
            INotifiable notifiable,
            IOptions<AppSettings> appSettings,
            ILogger<AuthController> logger,
            IUsuarioService usuarioService,
            UserManager<AspnetUserExtension> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AspnetUserExtension> signInManager) : base(notifiable)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _userManager = userManager;
            _usuarioService = usuarioService;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UsuarioLoginResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && !user.Active)
            {
                return Ok(new
                {
                    Authenticated = false,
                    Message = "Falha ao autenticar, usuário inativo"
                });
            }

            var resultado = await _signInManager.PasswordSignInAsync(user.UserName, model.Senha, false, true);

            if (resultado.Succeeded)
            {
                var tokenResponse = await GerarJwt(user.UserName);
                return Ok(tokenResponse);
            }

            return Ok(new
            {
                Authenticated = false,
                Message = "Falha ao autenticar, credenciais inválidas"
            });

            return CustomResponse();
        }

        [HttpPost("cadastro-admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> RegistroAdmin([FromBody] UsuarioRegistroModel model)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var identityUser = new AspnetUserExtension(model.Email, model.Nome);
                identityUser.Email = model.Email;
                identityUser.NormalizedEmail = model.Email.ToUpper();

                var resultado = await _userManager.CreateAsync(identityUser, model.Senha);

                if(resultado.Succeeded)
                {
                    await _usuarioService.InserirAsync(Guid.Parse(identityUser.Id), model.Nome, model.Email);

                    if(!_notifiable.HasNotification)
                    {
                        await CreateRoles();

                        resultado = await _userManager.AddToRoleAsync(identityUser, PerfilUsuarioEnum.Admin.ToString());

                        if (resultado.Succeeded)
                        {
                            return CustomResponse();
                        }

                    }

                    await _userManager.DeleteAsync(identityUser);

                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        _notifiable.AddNotification("Erro no registro", error.Description);
                    }
                }

                return CustomResponse();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }

        [HttpPost("cadastro-colaborador")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> RegistroColaborador([FromBody] UsuarioRegistroModel model)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var identityUser = new AspnetUserExtension(model.Email, model.Nome);
                identityUser.Email = model.Email;
                identityUser.NormalizedEmail = model.Email.ToUpper();

                var resultado = await _userManager.CreateAsync(identityUser, model.Senha);

                if (resultado.Succeeded)
                {
                    await _usuarioService.InserirAsync(Guid.Parse(identityUser.Id), model.Nome, model.Email);

                    if (!_notifiable.HasNotification)
                    {
                        await CreateRoles();

                        resultado = await _userManager.AddToRoleAsync(identityUser, PerfilUsuarioEnum.Colaborador.ToString());

                        if (resultado.Succeeded)
                        {
                            return CustomResponse();
                        }

                    }

                    await _userManager.DeleteAsync(identityUser);
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        _notifiable.AddNotification("Erro no registro", error.Description);
                    }
                }

                return CustomResponse();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro: {ex.Message}", ex);
                return InternalServerError($"Erro: {ex.Message}");
            }
        }

        private async Task CreateRoles()
        {
            string[] rolesNames = {
                PerfilUsuarioEnum.Admin.ToString(),
                PerfilUsuarioEnum.Colaborador.ToString()
            };

            foreach (var namesRole in rolesNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }
        }

        private async Task<UsuarioLoginResponseModel> GerarJwt(string email)
        {
            var identityUser = await _userManager.FindByNameAsync(email);
            var claims = await _userManager.GetClaimsAsync(identityUser);

            var identityClaims = await ObterClaimsUsuario(claims, identityUser);
            var encodedToken = CodificarToken(identityClaims);

            return await ObterRespostaToken(encodedToken, identityUser, claims);
        }

        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, AspnetUserExtension user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, ToUnixEpochDate(DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras)).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<UsuarioLoginResponseModel> ObterRespostaToken(string encodedToken, AspnetUserExtension usuario, IEnumerable<Claim> claims)
        {
            return new UsuarioLoginResponseModel
            {
                Authenticated = true,
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalHours,
                UsuarioToken = new UsuarioToken
                {
                    Id = usuario.Id.ToString(),
                    Email = usuario.Email,
                    Nome = usuario.Name,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
