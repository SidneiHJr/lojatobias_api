using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using LojaTobias.Domain.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Domain.Services
{
    public class ProdutoService : Service<Produto>, IProdutoService
    {
        private readonly IRepository<UnidadeMedida> _unidadeMedidaRepository;
        public ProdutoService(
            IRepository<Produto> repository,
            INotifiable notifiable,
            IUnitOfWork unitOfWork,
            IAspnetUser aspnetUser,
            IRepository<UnidadeMedida> unidadeMedidaRepository) : base(repository, notifiable, unitOfWork, aspnetUser)
        {
            _unidadeMedidaRepository = unidadeMedidaRepository;
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
    }
}
