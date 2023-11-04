
namespace LojaTobias.Api.Core.Models
{
    public class PedidoVendaModel
    {
        public string? Observacao { get; set; }
        public string? Cliente { get; set; }

        public IEnumerable<PedidoItemModel> Produtos { get; set; }
    }
}
