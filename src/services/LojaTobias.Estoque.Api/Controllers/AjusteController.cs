using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Estoque.Api.Controllers
{
    public class AjusteController : MainController
    {
        private readonly IAjusteService _ajusteService;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public AjusteController(INotifiable notifiable, IAjusteService ajusteService, IMapper mapper, IProdutoService produtoService) : base(notifiable)
        {
            _ajusteService = ajusteService;
            _mapper = mapper;
            _produtoService = produtoService;
        }

        [HttpGet("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AjusteResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarAjustes()
        {
            var resultado = await _ajusteService.GetAsync();

            return CustomResponse(resultado);
        }

        [HttpPost("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Insert([FromBody] AjusteModel model)
        {
            var ajusteId = await _ajusteService.InsertAsync(_mapper.Map<Ajuste>(model));

            if(_notifiable.HasNotification || ajusteId == Guid.Empty)
            {
                return CustomResponse();
            }

            if(model.Tipo == TipoAjusteEnum.Credito.ToString())
            {
                await _produtoService.AdicionarEstoquePeloAjusteAsync(ajusteId);
                await _ajusteService.InserirMovimentacaoAsync(TipoAjusteEnum.Credito.ToString(), ajusteId);

            }

            if (model.Tipo == TipoAjusteEnum.Debito.ToString())
            {
                await _produtoService.RemoverEstoquePeloAjusteAsync(ajusteId);
                await _ajusteService.InserirMovimentacaoAsync(TipoAjusteEnum.Debito.ToString(), ajusteId);
            }

            return CustomResponse(ajusteId);

        }

    }
}
