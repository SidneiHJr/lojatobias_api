using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using LojaTobias.Infra.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Identidade.Api.Controllers
{
    [Route("api/usuario")]
    public class UsuarioController : MainController
    {
        private readonly UserManager<AspnetUserExtension> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UsuarioController(
            INotifiable notifiable,
            UserManager<AspnetUserExtension> userManager,
            RoleManager<IdentityRole> roleManager,
            IUsuarioService usuarioService,
            IMapper mapper,
            IUnitOfWork unitOfWork) : base(notifiable)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _usuarioService = usuarioService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] UsuarioModel model)
        {
            //atualizar identity
            var identityUser = await _userManager.FindByIdAsync(id.ToString());

            if(identityUser == null)
            {
                _notifiable.AddNotification("Falha ao buscar usuário para atualização.");
                return CustomResponse();
            }

            _unitOfWork.BeginTransaction();

            identityUser.Atualizar(model.Nome, model.Email, model.Ativo);

            var resultado = await _userManager.UpdateAsync(identityUser);

            if (resultado.Succeeded)
            {
                await _usuarioService.UpdateAsync(id, _mapper.Map<Usuario>(model));
                await _unitOfWork.CommitTransactionAsync();
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    _notifiable.AddNotification("Erro no registro", error.Description);
                }
            }

            if (_notifiable.HasNotification)
                await _unitOfWork.RollBackAsync();

            return CustomResponse();
            //atualizar usuario
        }

    }
}
