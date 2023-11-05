using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(Context context) : base(context)
        {
        }

        public void UpdateMany(Pedido pedido)
        {
            var dbPedidoItems = _context.Set<PedidoItem>()
                                .Where(p => p.PedidoId == pedido.Id)
                                .AsNoTracking()
                                .ToList();

            _context.Upsert(pedido)
                .ThenUpdateMany(dbPedidoItems, p => p.Produtos);
        }
    }
}
