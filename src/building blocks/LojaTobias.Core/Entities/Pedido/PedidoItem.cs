using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Core.Entities
{
    public class PedidoItem: EntityBase
    {
        public decimal Quantidade { get; private set; }
        public decimal Valor { get; private set; }
        public string? Observacao { get; private set; }

        public Guid UnidadeMedidaId { get; private set; }
        public virtual UnidadeMedida UnidadeMedida { get; private set; }

        public Guid ProdutoId { get; private set; }
        public virtual Produto Produto { get; private set; }

        public Guid PedidoId { get; private set; }
        public virtual Pedido Pedido { get; private set; }
    }
}
