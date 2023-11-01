using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Domain.Services
{
    public class ProdutoService : Service<Produto>, IProdutoService
    {
        public ProdutoService(
            IRepository<Produto> repository, 
            INotifiable notifiable, 
            IUnitOfWork unitOfWork, 
            IAspnetUser aspnetUser) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
        }

        public async Task<IQueryable<Produto>> FiltrarAsync(string? termo, string? colunaOrdem, string direcaoOrdem)
        {
            var resultado = _repository.Table
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
    }
}
