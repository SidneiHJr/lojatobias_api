using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface ICaixaService : IService<Caixa>
    {
        Task<Caixa?> BuscarCaixa();
        Task<IEnumerable<Movimentacao>> BuscarMovimentacoes();
        Task<Guid> InserirCaixaAsync(decimal saldoInicial);
        Task InserirMovimentacaoAsync(string tipo, Guid pedidoId);
    }
}
