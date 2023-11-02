using AutoMapper;
using LojaTobias.Api.Core.Controllers;
using LojaTobias.Api.Core.Models;
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaTobias.Catalogo.Api.Controllers
{
    [Route("api/produto")]
    [Authorize]
    public class ProdutoController : CrudController<ProdutoModel, ProdutoResponseModel, Produto>
    {
        private readonly IProdutoService _produtoService;
        private readonly IService<UnidadeMedida> _unidadeMedidaService;
        public ProdutoController(
            IProdutoService service,
            IMapper mapper,
            INotifiable notifiable,
            IService<UnidadeMedida> unidadeMedidaService) : base(service, mapper, notifiable)
        {
            _produtoService = service;
            _unidadeMedidaService = unidadeMedidaService;
        }

        [HttpPost("filtro")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> Filtrar([FromBody] ProdutoFiltroModel filtro, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            var resultado = await _produtoService.FiltrarAsync(filtro.Termo, filtro.ColunaOrdem, filtro.DirecaoOrdem);

            var resultadoPaginado = PaginacaoListModel<Produto>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

            var resposta = _mapper.Map<IEnumerable<ProdutoResponseModel>>(resultadoPaginado);

            return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resposta);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ProdutoResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public override async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var resultado = _mapper.Map<ProdutoResponseModel>(await _produtoService.GetAsync(id));

            return CustomResponse(resultado);
        }

        [HttpPost("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public override async Task<IActionResult> Insert([FromBody] ProdutoModel item)
        {
            return await base.Insert(item);
        }

        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public override async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProdutoModel item)
        {
            return await base.Update(id, item);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public override async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return await base.Delete(id);
        }


        #region Unidade de Medida

        [HttpGet("unidade-medida")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UnidadeMedidaResponseModel>), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> BuscarUnidadesDeMedida()
        {
            var resultado = _mapper.Map<IEnumerable<UnidadeMedidaResponseModel>>(await _unidadeMedidaService.GetAsync());

            return CustomResponse(resultado);
        }

        [HttpGet("unidade-medida/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UnidadeMedidaResponseModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        public async Task<IActionResult> BuscarUnidadeMedida([FromRoute] Guid id)
        {
            var resultado = _mapper.Map<UnidadeMedidaResponseModel>(await _unidadeMedidaService.GetAsync(id));

            return CustomResponse(resultado);
        }

        [HttpPost("unidade-medida")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> InserirUnidadeMedida([FromBody] UnidadeMedidaModel model)
        {
            var unidadeMedidaCadastrada = await _produtoService.BuscarUnidadeMedidaPorNomeOuAbreviacao(model.Nome, model.Abreviacao);

            if (unidadeMedidaCadastrada != null)
            {
                _notifiable.AddNotification("Já existe uma unidade de medida cadastrada com esse nome ou abreviação");
                return CustomResponse();
            }

            var resultado = await _unidadeMedidaService.InsertAsync(_mapper.Map<UnidadeMedida>(model));

            return CustomResponse(resultado);
        }

        [HttpPut("unidade-medida/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarUnidadeMedida([FromRoute] Guid id, [FromBody] UnidadeMedidaModel model)
        {
            var unidadeMedida = await _unidadeMedidaService.GetAsync(id);

            if (unidadeMedida == null)
            {
                _notifiable.AddNotification("Falha ao buscar unidade de medida para atualização");
                return CustomResponse();
            }

            var dadosParaAtualizarExistentes = await _produtoService.BuscarUnidadeMedidaPorNomeOuAbreviacao(model.Nome, model.Abreviacao);

            if(dadosParaAtualizarExistentes != null)
            {
                _notifiable.AddNotification("Não é possível atualizar pois já existe uma unidade de medida cadastrada com esses dados");
                return CustomResponse();
            }

            await _unidadeMedidaService.UpdateAsync(id, _mapper.Map<UnidadeMedida>(model));

            return CustomResponse();
        }

        [HttpDelete("unidade-medida/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> RemoverUnidadeMedida([FromRoute] Guid id)
        {
            await _unidadeMedidaService.DeleteAsync(id);

            return CustomResponse();
        }
        #endregion
    }
}
