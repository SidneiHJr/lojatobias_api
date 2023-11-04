using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IPedidoService : IService<Pedido>
    {
        Task<Guid> InserirPedidoCompraAsync(Pedido pedido);
        Task FinalizarPedidoCompraAsync(Guid id);
    }
}
