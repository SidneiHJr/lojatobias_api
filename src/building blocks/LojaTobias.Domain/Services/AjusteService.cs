using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Domain.Services
{
    public class AjusteService : Service<Ajuste>, IAjusteService
    {
        private readonly IRepository<Produto> _produtoRepository;
        private readonly IRepository<UnidadeMedidaConversao> _unidadeMedidaConversaoRepository;
        private readonly IRepository<Movimentacao> _movimentacaoRepository;
        public AjusteService(
            IRepository<Ajuste> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IRepository<Produto> produtoRepository,
            IRepository<UnidadeMedidaConversao> unidadeMedidaConversaoRepository,
            IRepository<Movimentacao> movimentacaoRepository) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _produtoRepository = produtoRepository;
            _unidadeMedidaConversaoRepository = unidadeMedidaConversaoRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task InserirMovimentacaoAsync(string tipo, Guid ajusteId)
        {
            var ajuste = await _repository.Table
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(p => p.Id == ajusteId);

            var produto = await _produtoRepository.Table.FirstOrDefaultAsync(p => p.Id == ajuste.ProdutoId);

            decimal quantidade = ajuste.Quantidade;

            if (ajuste.UnidadeMedidaId != produto.UnidadeMedidaId)
            {
                var conversao = await _unidadeMedidaConversaoRepository.Table.FirstOrDefaultAsync(p => p.UnidadeMedidaEntradaId == ajuste.UnidadeMedidaId &&
                                                                                                       p.UnidadeMedidaSaidaId == produto.UnidadeMedidaId);

                quantidade = ajuste.Quantidade * conversao.FatorConversao;
            }

            var movimentacao = new Movimentacao(CategoriaMovimentacaoEnum.Estoque.ToString(), tipo);

            movimentacao.NovaMovimentacaoEstoque(null, ajuste.Id, produto.Id, quantidade);

            ValidarMovimentacao(movimentacao);

            if (_notifiable.HasNotification)
                return;

            movimentacao.NovaInsercao(string.IsNullOrEmpty(movimentacao.UsuarioCriacao) ? _aspnetUser.GetUserId() : movimentacao.UsuarioCriacao);

            await _movimentacaoRepository.InsertAsync(movimentacao);
            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
            {
                _notifiable.AddNotification("Inserir", "Erro ao inserir o registro");
                await _unitOfWork.RollBackAsync();
                return;
            }
        }

        private void ValidarMovimentacao(Movimentacao movimentacao)
        {
            var erros = movimentacao.Validar();
            _notifiable.AddNotifications(erros);
        }
    }
}
