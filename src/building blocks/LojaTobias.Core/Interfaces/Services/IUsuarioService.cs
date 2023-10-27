using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task InserirAsync(Guid id, string nome, string email);
    }
}
