using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<IQueryable<Usuario>> FiltrarAsync(string? termo, string? colunaOrdem, string direcaoOrdem);
        Task InserirAsync(Guid id, string nome, string email, string perfil);
    }
}
