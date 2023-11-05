using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        void UpdateMany(Pedido pedido);
    }
}
