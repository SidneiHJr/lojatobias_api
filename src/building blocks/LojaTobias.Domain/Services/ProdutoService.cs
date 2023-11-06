using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Domain.Services
{
    public class ProdutoService : Service<Produto>, IProdutoService
    {
        private readonly IRepository<UnidadeMedida> _unidadeMedidaRepository;
        private readonly IRepository<UnidadeMedidaConversao> _unidadeMedidaConversaoRepository;
        private readonly IRepository<Pedido> _pedidoRepository;
        private readonly IRepository<Ajuste> _ajusteRepository;
        public ProdutoService(
            IRepository<Produto> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IRepository<UnidadeMedida> unidadeMedidaRepository,
            IRepository<UnidadeMedidaConversao> unidadeMedidaConversaoRepository,
            IRepository<Pedido> pedidoRepository,
            IRepository<Ajuste> ajusteRepository) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _unidadeMedidaRepository = unidadeMedidaRepository;
            _unidadeMedidaConversaoRepository = unidadeMedidaConversaoRepository;
            _pedidoRepository = pedidoRepository;
            _ajusteRepository = ajusteRepository;
        }

        public async Task<IQueryable<Produto>> FiltrarAsync(string? termo, string? colunaOrdem, string direcaoOrdem)
        {
            var resultado = _repository.Table
                                            .Include(p => p.UnidadeMedida)
                                            .Where(p => (string.IsNullOrEmpty(termo) ||
                                                        p.Nome.ToUpper().Contains(termo.ToUpper())) &&
                                                        p.Removido == false)
                                            .AsNoTracking();

            if (!string.IsNullOrEmpty(colunaOrdem))
            {
                resultado = resultado.OrderBy(colunaOrdem, direcaoOrdem);
            }
            else
                resultado = resultado.OrderByProp(p => p.Nome, direcaoOrdem);

            return await Task.FromResult(resultado);

        }

        public override async Task<Produto?> GetAsync(Guid id)
        {
            var resultado = await _repository.Table
                                                .Include(p => p.UnidadeMedida)
                                                .FirstOrDefaultAsync(p => p.Id == id);

            return resultado;
        }

        public async Task<UnidadeMedida?> BuscarUnidadeMedidaPorNomeOuAbreviacao(string nome, string abreviacao)
        {
            var unidadeMedida = await _unidadeMedidaRepository.Table
                                                                .FirstOrDefaultAsync(p => (p.Nome.ToUpper() == nome.ToUpper() || 
                                                                                          p.Abreviacao.ToLower() == abreviacao.ToLower()) &&
                                                                                          p.Removido == false);

            return unidadeMedida;
        }

        public async Task<Guid> InserirUnidadeMedidaConversao(UnidadeMedidaConversao entidade)
        {
            ValidarUnidadeMedidaConversao(entidade);

            if (_notifiable.HasNotification)
                return Guid.Empty;

            var conversaoExistente = await _unidadeMedidaConversaoRepository.Table.FirstOrDefaultAsync(p => p.UnidadeMedidaEntradaId == entidade.UnidadeMedidaEntradaId &&
                                                                                                            p.UnidadeMedidaSaidaId == entidade.UnidadeMedidaSaidaId);

            if(conversaoExistente != null)
            {
                _notifiable.AddNotification("Conversão já existente");
                return Guid.Empty;
            }

            entidade.NovaInsercao(string.IsNullOrEmpty(entidade.UsuarioCriacao) ? _aspnetUser.GetUserId() : entidade.UsuarioCriacao);

            await _unidadeMedidaConversaoRepository.InsertAsync(entidade);
            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
            {
                _notifiable.AddNotification("Erro ao inserir unidade de medida conversão");
                return Guid.Empty;
            }

            return entidade.Id;
        }

        public async Task AdicionarEstoquePeloPedidoAsync(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.Table
                                                    .Include(p => p.Produtos)
                                                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if(pedido == null)
            {
                _notifiable.AddNotification("Falha ao buscar o pedido");
                return;
            }

            if(pedido.Status != StatusPedidoEnum.Realizado.ToString())
            {
                _notifiable.AddNotification($"Não é possível finalizar o pedido pois ele está {pedido.Status}");
                return;
            }

            if(pedido.Tipo != TipoPedidoEnum.Compra.ToString())
            {
                _notifiable.AddNotification("O pedido não é um pedido de compra");
                return;
            }

            _unitOfWork.BeginTransaction();

            foreach(var pedidoItem in pedido.Produtos)
            {

                var produto = await _repository.Table
                                                    .Include(p => p.UnidadeMedida)
                                                    .FirstOrDefaultAsync(p => p.Id == pedidoItem.ProdutoId);

                if (produto == null)
                {
                    _notifiable.AddNotification("Falha ao buscar produto do pedido");
                }
                else
                {
                    decimal quantidadeAdicionar = pedidoItem.Quantidade;

                    if (pedidoItem.UnidadeMedidaId != produto.UnidadeMedidaId)
                    {

                        quantidadeAdicionar = await CalcularConversao(pedidoItem.Quantidade, pedidoItem.UnidadeMedidaId, produto.UnidadeMedidaId);

                        if (_notifiable.HasNotification)
                        {
                            await _unitOfWork.RollBackAsync();
                            return;
                        }
                    }

                    produto.AdicionarQuantidade(quantidadeAdicionar);

                    _repository.Update(produto);
                    await _unitOfWork.CommitAsync();
                }
            }

            if (_notifiable.HasNotification)
            {
                await _unitOfWork.RollBackAsync();
                return;
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task AdicionarEstoquePeloAjusteAsync(Guid ajusteId)
        {
            var ajuste = await _ajusteRepository.Table
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(p => p.Id == ajusteId);

            var produto = await _repository.Table
                                                .FirstOrDefaultAsync(p => p.Id == ajuste.ProdutoId);

            decimal quantidadeAdicionar = ajuste.Quantidade;

            if (ajuste.UnidadeMedidaId != produto.UnidadeMedidaId)
            {

                quantidadeAdicionar = await CalcularConversao(ajuste.Quantidade, ajuste.UnidadeMedidaId, produto.UnidadeMedidaId);

                if (_notifiable.HasNotification)
                    return;
            }

            produto.AdicionarQuantidade(quantidadeAdicionar);

            _repository.Update(produto);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoverEstoquePeloPedidoAsync(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.Table
                                                    .Include(p => p.Produtos)
                                                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
            {
                _notifiable.AddNotification("Falha ao buscar o pedido");
                return;
            }

            if (pedido.Status != StatusPedidoEnum.Realizado.ToString())
            {
                _notifiable.AddNotification($"Não é possível finalizar o pedido pois ele está {pedido.Status}");
                return;
            }

            if (pedido.Tipo != TipoPedidoEnum.Venda.ToString())
            {
                _notifiable.AddNotification("O pedido não é um pedido de venda");
                return;
            }

            _unitOfWork.BeginTransaction();

            foreach (var pedidoItem in pedido.Produtos)
            {

                var produto = await _repository.Table
                                                    .Include(p => p.UnidadeMedida)
                                                    .FirstOrDefaultAsync(p => p.Id == pedidoItem.ProdutoId);

                if (produto == null)
                {
                    _notifiable.AddNotification("Falha ao buscar produto do pedido");
                    await _unitOfWork.RollBackAsync();
                    return;
                }

                decimal quantidadeVenda = pedidoItem.Quantidade;

                if (pedidoItem.UnidadeMedidaId != produto.UnidadeMedidaId)
                {

                    quantidadeVenda = await CalcularConversao(pedidoItem.Quantidade, pedidoItem.UnidadeMedidaId, produto.UnidadeMedidaId);

                    if (_notifiable.HasNotification)
                    {
                        await _unitOfWork.RollBackAsync();
                        return;
                    }
                }

                if (produto.Quantidade < quantidadeVenda)
                {
                    _notifiable.AddNotification($"O produto {produto.Nome} não tem estoque suficiente. Estoque disponível {produto.Quantidade}{produto.UnidadeMedida.Abreviacao}");
                    await _unitOfWork.RollBackAsync();
                    return;
                }

                produto.RemoverQuantidade(quantidadeVenda);

                _repository.Update(produto);
                await _unitOfWork.CommitAsync();
                
            }

            if (_notifiable.HasNotification)
            {
                await _unitOfWork.RollBackAsync();
                return;
            }

            await _unitOfWork.CommitTransactionAsync();
        }

        public async Task RemoverEstoquePeloAjusteAsync(Guid ajusteId)
        {
            var ajuste = await _ajusteRepository.Table
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(p => p.Id == ajusteId);

            var produto = await _repository.Table
                                                .FirstOrDefaultAsync(p => p.Id == ajuste.ProdutoId);

            decimal quantidadeAjuste = ajuste.Quantidade;

            if (ajuste.UnidadeMedidaId != produto.UnidadeMedidaId)
            {

                quantidadeAjuste = await CalcularConversao(ajuste.Quantidade, ajuste.UnidadeMedidaId, produto.UnidadeMedidaId);

                if (_notifiable.HasNotification)
                    return;
            }

            if (produto.Quantidade < quantidadeAjuste)
            {
                _notifiable.AddNotification($"O produto {produto.Nome} não tem estoque suficiente. Estoque disponível {produto.Quantidade}{produto.UnidadeMedida.Abreviacao}");
                await _unitOfWork.RollBackAsync();
                return;
            }

            produto.RemoverQuantidade(quantidadeAjuste);

            _repository.Update(produto);
            await _unitOfWork.CommitAsync();
        }

        private void ValidarUnidadeMedidaConversao(UnidadeMedidaConversao entidade)
        {
            var erros = entidade.Validar();
            _notifiable.AddNotifications(erros);
        }

        private async Task<decimal> CalcularConversao(decimal quantidade, Guid unidadeMedidaEntradaId, Guid unidadeMedidaSaidaId)
        {
            decimal quantidadeConvertida = 0;

            var conversao = await _unidadeMedidaConversaoRepository.Table.FirstOrDefaultAsync(p => p.UnidadeMedidaEntradaId == unidadeMedidaEntradaId &&
                                                                                                   p.UnidadeMedidaSaidaId == unidadeMedidaSaidaId);

            if (conversao == null)
            {
                _notifiable.AddNotification("Não existe conversão para unidade de medida informada");
                return 0;
            }

            quantidadeConvertida = quantidade * conversao.FatorConversao;

            return quantidadeConvertida;
        }
    }
}
