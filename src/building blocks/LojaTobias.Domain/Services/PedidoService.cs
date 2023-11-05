using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Domain.Services
{
    public class PedidoService : Service<Pedido>, IPedidoService
    {
        private readonly IRepository<Produto> _produtoRepository;
        private readonly IRepository<UnidadeMedidaConversao> _unidadeMedidaConversaoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoService(
            IPedidoRepository repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IRepository<Produto> produtoRepository,
            IRepository<UnidadeMedidaConversao> unidadeMedidaConversaoRepository) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _produtoRepository = produtoRepository;
            _unidadeMedidaConversaoRepository = unidadeMedidaConversaoRepository;
            _pedidoRepository = repository;
        }

        public override async Task UpdateAsync(Guid id, Pedido entity)
        {
            await Validate(entity);

            if (_notifiable.HasNotification)
                return;

            var pedido = await _repository.GetAsync(id);

            if(pedido == null)
            {
                _notifiable.AddNotification("Falha ao buscar pedido");
                return;
            }

            pedido.Atualizar(entity);

            pedido.NovaAtualizacao(_aspnetUser.GetUserId());

            _pedidoRepository.UpdateMany(pedido);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IQueryable<Pedido>> FiltrarAsync(string? termo, string? tipo, string? status, string? colunaOrdem, string direcaoOrdem)
        {
            var resultado = _repository.Table
                                            .Include(p => p.Produtos)
                                            .Where(p => string.IsNullOrEmpty(termo) ||
                                                        !string.IsNullOrEmpty(p.Observacao) && p.Observacao.ToUpper().Contains(termo.ToUpper()) ||
                                                        !string.IsNullOrEmpty(p.Fornecedor) && p.Fornecedor.ToUpper().Contains(termo.ToUpper()) ||
                                                        !string.IsNullOrEmpty(p.Cliente) && p.Cliente.ToUpper().Contains(termo.ToUpper()))
                                            .AsNoTracking();

            if (!string.IsNullOrEmpty(tipo))
            {
                resultado = resultado.Where(p => p.Tipo.ToUpper() == tipo.ToUpper());
            }

            if(!string.IsNullOrEmpty(status))
            {
                resultado = resultado.Where(p => p.Status.ToUpper() == status.ToUpper());   
            }

            if (!string.IsNullOrEmpty(colunaOrdem))
            {
                resultado = resultado.OrderBy(colunaOrdem, direcaoOrdem);
            }
            else
                resultado = resultado.OrderByProp(p => p.DataCriacao.Value, direcaoOrdem);

            return await Task.FromResult(resultado);

        }
        public async Task<Pedido> BuscarPedidoAsync(Guid id)
        {
            var resultado = await _pedidoRepository.Table
                                                        .Include(p => p.Produtos)
                                                        .FirstOrDefaultAsync(p => p.Id == id);

            return resultado;
        }

        public async Task FinalizarPedidoAsync(Guid id)
        {
            var pedido = await _repository.Table
                                            .Include(p => p.Produtos)
                                            .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                _notifiable.AddNotification("Falha ao buscar pedido");
                return;
            }

            pedido.AtualizarStatus(StatusPedidoEnum.Finalizado.ToString());

            _repository.Update(pedido);
            await _unitOfWork.CommitAsync();
        }

        public async Task CancelarPedidoAsync(Guid id)
        {
            var pedido = await _repository.GetAsync(id);

            if (pedido == null)
            {
                _notifiable.AddNotification("Falha ao buscar pedido");
                return;
            }

            pedido.AtualizarStatus(StatusPedidoEnum.Cancelado.ToString());

            _repository.Update(pedido);
            await _unitOfWork.CommitAsync();
        }

        #region Pedido de Compra

        public async Task<Guid> InserirPedidoCompraAsync(Pedido pedido)
        {
            pedido.NovoPedido(TipoPedidoEnum.Compra.ToString());

            decimal totalPedido = 0;

            foreach(var pedidoItem in pedido.Produtos)
            {
                var produto = await _produtoRepository.Table
                                                        .Include(p => p.UnidadeMedida)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(p => p.Id == pedidoItem.ProdutoId);

                if(produto == null)
                {
                    _notifiable.AddNotification("Falha ao buscar produto do pedido");
                    return Guid.Empty;
                }

                if(pedidoItem.UnidadeMedidaId != produto.UnidadeMedidaId)
                {
                    var conversao = await _unidadeMedidaConversaoRepository.Table
                                                                                .AsNoTracking()
                                                                                .FirstOrDefaultAsync(p => p.UnidadeMedidaEntradaId == pedidoItem.UnidadeMedidaId &&
                                                                                                          p.UnidadeMedidaSaidaId == produto.UnidadeMedidaId);

                    if(conversao == null)
                    {
                        _notifiable.AddNotification($"Não existe uma conversão válida para o produto {produto.Nome}");
                        return Guid.Empty;
                    }
                }

                totalPedido += pedidoItem.Valor;
            }

            pedido.AtualizarTotal(totalPedido);

            var resultado = await InsertAsync(pedido);

            return resultado;
        }

        #endregion

        #region Pedido de Venda

        public async Task<Guid> InserirPedidoVendaAsync(Pedido pedido)
        {
            pedido.NovoPedido(TipoPedidoEnum.Venda.ToString());

            decimal totalPedido = 0;

            foreach (var pedidoItem in pedido.Produtos)
            {
                var produto = await _produtoRepository.Table
                                                        .Include(p => p.UnidadeMedida)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(p => p.Id == pedidoItem.ProdutoId);

                if (produto == null)
                {
                    _notifiable.AddNotification("Falha ao buscar produto do pedido");
                    return Guid.Empty;
                }

                var quantidadeItem = pedidoItem.Quantidade;


                if (pedidoItem.UnidadeMedidaId != produto.UnidadeMedidaId)
                {
                    var conversao = await _unidadeMedidaConversaoRepository.Table
                                                                                .AsNoTracking()
                                                                                .FirstOrDefaultAsync(p => p.UnidadeMedidaEntradaId == pedidoItem.UnidadeMedidaId &&
                                                                                                          p.UnidadeMedidaSaidaId == produto.UnidadeMedidaId);

                    if (conversao == null)
                    {
                        _notifiable.AddNotification($"Não existe uma conversão válida para o produto {produto.Nome}");
                        return Guid.Empty;
                    }

                    quantidadeItem = pedidoItem.Quantidade * conversao.FatorConversao;
                }

                //Checar se tem estoque
                if(produto.Quantidade < quantidadeItem)
                    _notifiable.AddNotification($"O produto {produto.Nome} não tem estoque suficiente. Estoque disponível {produto.Quantidade}{produto.UnidadeMedida.Abreviacao}");

                totalPedido += pedidoItem.Valor;
            }

            if (_notifiable.HasNotification)
                return Guid.Empty;

            pedido.AtualizarTotal(totalPedido);

            var resultado = await InsertAsync(pedido);

            return resultado;
        }

        #endregion
    }
}
