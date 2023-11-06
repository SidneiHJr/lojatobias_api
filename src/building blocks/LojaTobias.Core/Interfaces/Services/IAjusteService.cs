using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IAjusteService : IService<Ajuste>
    {
        Task InserirMovimentacaoAsync(string tipo, Guid ajusteId);
    }
}
