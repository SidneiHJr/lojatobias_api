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
        public ProdutoController(
            IProdutoService service, 
            IMapper mapper,
            INotifiable notifiable) : base(service, mapper, notifiable)
        {
            _produtoService = service;
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

    }
}
