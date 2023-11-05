using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IPedidoService : IService<Pedido>
    {
        Task<IQueryable<Pedido>> FiltrarAsync(string? termo, string tipo, string? status, string? colunaOrdem, string direcaoOrdem);
        Task<Pedido> BuscarPedidoAsync(Guid id);
        Task FinalizarPedidoAsync(Guid id);
        Task CancelarPedidoAsync(Guid id);

        Task<Guid> InserirPedidoCompraAsync(Pedido pedido);
        Task<Guid> InserirPedidoVendaAsync(Pedido pedido);
    }
}
