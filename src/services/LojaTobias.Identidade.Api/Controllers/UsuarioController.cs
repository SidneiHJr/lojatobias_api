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

        [HttpPost("filtro")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Filtrar([FromBody] UsuarioFiltroModel filtro, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            var resultado = await _usuarioService.FiltrarAsync(filtro.Termo, filtro.ColunaOrdem, filtro.DirecaoOrdem);

            var resultadoPaginado = PaginacaoListModel<Usuario>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

            var resposta = _mapper.Map<IEnumerable<UsuarioResponseModel>>(resultadoPaginado);

            return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resposta);
        }


        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Atualizar([FromRoute] Guid id, [FromBody] UsuarioModel model)
        {
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

                if (!_notifiable.HasNotification)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return CustomResponse();
                }
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    _notifiable.AddNotification("Erro no registro", error.Description);
                }
            }

            await _unitOfWork.RollBackAsync();

            return CustomResponse();
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Remover([FromRoute] Guid id)
        {
            var usuario = await _usuarioService.GetAsync(id);

            if(usuario == null)
            {
                _notifiable.AddNotification("Usuário para remoção não encontrado.");
                return CustomResponse();
            }

            if (usuario.Removido)
            {
                _notifiable.AddNotification("Usuário já foi removido.");
                return CustomResponse();
            }


            var identityUser = await _userManager.FindByIdAsync(id.ToString());

            if (identityUser == null)
            {
                _notifiable.AddNotification("Falha ao buscar usuário para remoção.");
                return CustomResponse();
            }

            _unitOfWork.BeginTransaction();

            var resultado = await _userManager.DeleteAsync(identityUser);

            if (resultado.Succeeded)
            {
                await _usuarioService.DeleteAsync(id);

                if (!_notifiable.HasNotification)
                {
                    await _unitOfWork.CommitTransactionAsync();
                    return CustomResponse();
                }
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    _notifiable.AddNotification("Erro no registro", error.Description);
                }
            }

            await _unitOfWork.RollBackAsync();

            return CustomResponse();
        }
    }
}
