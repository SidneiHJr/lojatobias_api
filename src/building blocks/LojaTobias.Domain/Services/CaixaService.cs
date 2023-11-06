
using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Domain.Services
{
    public class CaixaService : Service<Caixa>, ICaixaService
    {
        private readonly IRepository<Movimentacao> _movimentacaoRepository;
        private readonly IRepository<Pedido> _pedidoRepository;
        private readonly IRepository<Ajuste> _ajusteRepository;

        public CaixaService(
            IRepository<Caixa> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IRepository<Movimentacao> movimentacaoRepository,
            IRepository<Pedido> pedidoRepository,
            IRepository<Ajuste> ajusteRepository) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _movimentacaoRepository = movimentacaoRepository;
            _pedidoRepository = pedidoRepository;
            _ajusteRepository = ajusteRepository;
        }

        public async Task<Caixa?> BuscarCaixa()
        {
            var resultado = await _repository.Table.FirstOrDefaultAsync();

            return resultado;
        }

        public async Task<IEnumerable<Movimentacao>> BuscarMovimentacoes()
        {
            var caixa = await _repository.Table
                                            .Include(p => p.Movimentacoes)
                                            .FirstOrDefaultAsync();

            if(caixa == null)
            {
                _notifiable.AddNotification("Falha ao buscar movimentações. Caixa não foi criado");
                return Enumerable.Empty<Movimentacao>();
            }

            var resultado =  _movimentacaoRepository.Table.Where(p => p.CaixaId == caixa.Id).OrderByProp(p => p.DataCriacao.Value, "desc");

            return await Task.FromResult(resultado);

        }

        public async Task<Guid> InserirCaixaAsync(decimal saldoInicial)
        {
            var caixaExistente = await _repository.Table.FirstOrDefaultAsync();

            if(caixaExistente != null)
            {
                _notifiable.AddNotification("Já existe um caixa criado");
                return Guid.Empty;
            }

            if(saldoInicial < 0)
            {
                _notifiable.AddNotification("Por favor informe um saldo inicial maior que zero");
                return Guid.Empty;
            }
            var caixa = new Caixa(saldoInicial);

            caixa.NovaInsercao(string.IsNullOrEmpty(caixa.UsuarioCriacao) ? _aspnetUser.GetUserId() : caixa.UsuarioCriacao);

            await _repository.InsertAsync(caixa);
            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
            {
                _notifiable.AddNotification("Falha ao criar novo caixa");
                return Guid.Empty;
            }

            return caixa.Id;
        }

        public async Task InserirMovimentacaoAsync(string tipo, Guid pedidoId)
        {
            var caixa = await _repository.Table
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

            if(caixa == null)
            {
                _notifiable.AddNotification("Nenhum caixa foi criado.");
                return;
            }

            _unitOfWork.BeginTransaction();

            var pedido = await _pedidoRepository.Table
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(p => p.Id == pedidoId);

            var movimentacao = new Movimentacao(CategoriaMovimentacaoEnum.Financeiro.ToString(), tipo);

            movimentacao.NovaMovimentacaoFinanceiro(caixa.Id, pedido.Id, pedido.Total);

            ValidarMovimentacao(movimentacao);

            if (_notifiable.HasNotification)
            {
                await _unitOfWork.RollBackAsync();
                return;
            }

            movimentacao.NovaInsercao(string.IsNullOrEmpty(movimentacao.UsuarioCriacao) ? _aspnetUser.GetUserId() : movimentacao.UsuarioCriacao);

            await _movimentacaoRepository.InsertAsync(movimentacao);
            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
            {
                _notifiable.AddNotification("Inserir", "Erro ao inserir o registro");
                await _unitOfWork.RollBackAsync();
                return;
            }

            if (tipo == TipoMovimentacaoEnum.Debito.ToString())
                await SubtrairSaldo(pedido.Total);

            if (tipo == TipoMovimentacaoEnum.Credito.ToString())
                await SomarSaldo(pedido.Total);

            await _unitOfWork.CommitTransactionAsync();
        }

        private async Task SomarSaldo(decimal valor)
        {
            var caixa = await _repository.Table.FirstOrDefaultAsync();

            caixa.AdicionarSaldo(valor);

            _repository.Update(caixa);
            await _unitOfWork.CommitAsync();
        }

        private async Task SubtrairSaldo(decimal valor)
        {
            var caixa = await _repository.Table.FirstOrDefaultAsync();

            caixa.SubtrairSaldo(valor);

            _repository.Update(caixa);
            await _unitOfWork.CommitAsync();
        }

        private void ValidarMovimentacao(Movimentacao movimentacao)
        {
            var erros = movimentacao.Validar();
            _notifiable.AddNotifications(erros);
        }

    }
}
