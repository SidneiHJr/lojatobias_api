
using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Identidade.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        private readonly ILogger _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(
            INotifiable notifiable,
            ILogger<AuthController> logger,
            UserManager<IdentityUser> userManager,
            IUsuarioService usuarioService,
            IMapper mapper) : base(notifiable)
        {
            _logger = logger;
            _userManager = userManager;
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        [HttpPost("cadastro")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroModel model)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                var identityUser = new IdentityUser(model.Email);

                var resultado = await _userManager.CreateAsync(identityUser, model.Senha);

                if(resultado.Succeeded)
                {
                    await _usuarioService.InserirAsync(Guid.Parse(identityUser.Id), model.Nome, model.Email);

                    if(!_notifiable.HasNotification)
                    {

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
    }
}
