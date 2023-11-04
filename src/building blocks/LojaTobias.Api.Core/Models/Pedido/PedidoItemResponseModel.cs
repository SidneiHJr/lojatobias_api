namespace LojaTobias.Api.Core.Models
{
    public class PedidoItemResponseModel
    {
        public Guid Id { get; set; }
        public string? Quantidade { get; set; }
        public string? Observacao { get; set; }

        public UnidadeMedidaResponseModel UnidadeMedida { get; set; }
        public ProdutoResponseModel Produto { get; set; }
        public PedidoResponseModel Pedido { get; set; }
    }
}
