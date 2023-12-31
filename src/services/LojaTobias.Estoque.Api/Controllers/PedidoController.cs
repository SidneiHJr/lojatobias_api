﻿using AutoMapper;
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
        private readonly ICaixaService _caixaService;
        private readonly IMapper _mapper;
        public PedidoController(
            INotifiable notifiable, IPedidoService pedidoService, IMapper mapper, IProdutoService produtoService, ICaixaService caixaService) : base(notifiable)
        {
            _pedidoService = pedidoService;
            _mapper = mapper;
            _produtoService = produtoService;
            _caixaService = caixaService;
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

        [HttpPost("cancelar/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CancelarPedido([FromRoute] Guid id)
        {
            await _pedidoService.CancelarPedidoAsync(id);

            return CustomResponse();
        }

        #region Pedido de Compra

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
            await _produtoService.AdicionarEstoquePeloPedidoAsync(id);

            if (_notifiable.HasNotification)
                return CustomResponse();

            await _pedidoService.InserirMovimentacoesAsync(TipoMovimentacaoEnum.Debito.ToString(), id);
            await _caixaService.InserirMovimentacaoAsync(TipoMovimentacaoEnum.Debito.ToString(), id);

            if (_notifiable.HasNotification)
                return CustomResponse();

            await _pedidoService.FinalizarPedidoAsync(id);


            return CustomResponse();
        }

        #endregion

        #region Pedido de Venda

        [HttpPost("venda")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador, Colaborador")]
        public async Task<IActionResult> InserirPedidoVenda([FromBody] PedidoVendaModel model)
        {
            var resultado = await _pedidoService.InserirPedidoVendaAsync(_mapper.Map<Pedido>(model));

            return CustomResponse(resultado);

        }

        [HttpPost("venda/{id}/atualizar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador, Colaborador")]
        public async Task<IActionResult> AtualizarPedidoVenda([FromRoute] Guid id, [FromBody] PedidoVendaModel model)
        {
            await _pedidoService.UpdateAsync(id, _mapper.Map<Pedido>(model));

            return CustomResponse();
        }

        [HttpPost("venda/{id}/finalizar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OkModel), 200)]
        [ProducesResponseType(typeof(BadRequestModel), 400)]
        [ProducesResponseType(typeof(InternalServerErrorModel), 500)]
        [Authorize(Roles = "Administrador, Colaborador")]
        public async Task<IActionResult> FinalizarPedidoVenda([FromRoute] Guid id)
        {
            await _produtoService.RemoverEstoquePeloPedidoAsync(id);

            if (_notifiable.HasNotification)
                return CustomResponse();

            await _pedidoService.InserirMovimentacoesAsync(TipoMovimentacaoEnum.Credito.ToString(), id);
            await _caixaService.InserirMovimentacaoAsync(TipoMovimentacaoEnum.Credito.ToString(), id);

            if (_notifiable.HasNotification)
                return CustomResponse();

            await _pedidoService.FinalizarPedidoAsync(id);

            return CustomResponse();
        }

        #endregion
    }
}
