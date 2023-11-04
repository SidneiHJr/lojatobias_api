using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class PedidoCompraModel
    {
        public string? Observacao { get; set; }

        [Required(ErrorMessage = "Por favor informe o fornecedor")]
        public string Fornecedor { get; set; }

        public IEnumerable<PedidoItemModel> Produtos { get; set; }
    }
}
