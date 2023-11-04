namespace LojaTobias.Api.Core.Models
{
    public class PedidoItemModel
    {
        public string? Observacao { get; set;}
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set;}
        public Guid UnidadeMedidaId { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid PedidoId { get; set; }
    }
}
