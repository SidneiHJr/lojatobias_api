using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IQueryable<Usuario>> FiltrarAsync(string? termo, string? colunaOrdem, string direcaoOrdem)
        {
            var resultado = _repository.Table
                                            .Where(p => (string.IsNullOrEmpty(termo) ||
                                                        p.Nome.ToUpper().Contains(termo.ToUpper()) ||
                                                        p.Email.Endereco.ToUpper().Contains(termo.ToUpper())) &&
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

        public async Task InserirAsync(Guid id, string nome, string email, string perfil)
        {
            var usuario = new Usuario(id, nome, email, perfil);

           await InsertAsync(usuario);

        }

    }
}
