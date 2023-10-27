using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;

namespace LojaTobias.Domain.Services
{
    public class UsuarioService : Service<Usuario>, IUsuarioService
    {
        public UsuarioService(
            IRepository<Usuario> repository, 
            INotifiable notifiable, 
            IUnitOfWork unitOfWork, 
            IAspnetUser aspnetUser) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
        }

        public async Task InserirAsync(Guid id, string nome, string email)
        {
            var usuario = new Usuario(id, nome, email);

           await InsertAsync(usuario);

        }
    }
}
