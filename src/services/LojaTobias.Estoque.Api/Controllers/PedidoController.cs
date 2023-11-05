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
    [Route("api/pedido")]
    public class PedidoController : MainController
    {
        private readonly IPedidoService _pedidoService;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public PedidoController(
            INotifiable notifiable, IPedidoService pedidoService, IMapper mapper, IProdutoService produtoService) : base(notifiable)
        {
            _pedidoService = pedidoService;
            _mapper = mapper;
            _produtoService = produtoService;
        }


        [HttpPost("filtro")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Filtrar([FromBody] PedidoFiltroModel filtro, [FromQuery] PaginacaoQueryStringModel paginacao)
        {
            var resultado = await _pedidoService.FiltrarAsync(filtro.Termo, filtro.Tipo, filtro.Status, filtro.ColunaOrdem, filtro.DirecaoOrdem);

            var resultadoPaginado = PaginacaoListModel<Pedido>.Create(resultado, paginacao.NumeroPagina, paginacao.TamanhoPagina);

            var resposta = _mapper.Map<IEnumerable<PedidoResponseModel>>(resultadoPaginado);

            return PagingResponse(resultadoPaginado.NumeroPagina, resultadoPaginado.Total, resultadoPaginado.TotalPaginas, resposta);
        }


        [HttpGet("compra/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarPedidoCompra([FromRoute] Guid id)
        {
            var resultado = await _pedidoService.BuscarPedidoAsync(id);

            return CustomResponse(resultado);

        }

        [HttpPost("compra")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> InserirPedidoCompra([FromBody] PedidoCompraModel model)
        {
            var resultado = await _pedidoService.InserirPedidoCompraAsync(_mapper.Map<Pedido>(model));

            return CustomResponse(resultado);

        }

        [HttpPost("compra/{id}/atualizar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarPedidoCompra([FromRoute] Guid id, [FromBody] PedidoCompraModel model)
        {
            await _pedidoService.UpdateAsync(id, _mapper.Map<Pedido>(model));

            return CustomResponse();
        }

        [HttpPost("compra/{id}/finalizar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> FinalizarPedidoCompra([FromRoute] Guid id)
        {
            await _produtoService.InserirProdutosPeloPedidoAsync(id);

            if (!_notifiable.HasNotification)
                await _pedidoService.FinalizarPedidoCompraAsync(id);

            return CustomResponse();
        }

        [HttpPost("compra/{id}/cancelar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CancelarPedidoCompra([FromRoute] Guid id)
        {
            await _pedidoService.CancelarPedidoCompraAsync(id);

            return CustomResponse();
        }
        
    }
}
