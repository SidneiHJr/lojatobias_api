using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IProdutoService : IService<Produto>
    {
        Task<IQueryable<Produto>> FiltrarAsync(string? termo, string? colunaOrdem, string direcaoOrdem);
        Task<UnidadeMedida?> BuscarUnidadeMedidaPorNomeOuAbreviacao(string nome, string abreviacao);
        Task<Guid> InserirUnidadeMedidaConversao(UnidadeMedidaConversao entidade);
        Task InserirProdutosPeloPedidoAsync(Guid pedidoId);
    }
}
