namespace LojaTobias.Api.Core.Models
{
    public class PedidoResponseModel
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; }
        public string? Observacao { get; set; }
        public decimal Total { get; set; }
        public string? Fornecedor { get; set; }
        public string? Cliente { get; set; }

        public IEnumerable<PedidoItemResponseModel> Produtos { get; set; }
    }
}
