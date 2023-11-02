using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class ProdutoModel
    {
        [Required(ErrorMessage = "Por favor informe um nome para o produto")]
        public required string Nome { get; set; }
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "Por favor informe o id da unidade de medida do produto")]
        public Guid UnidadeMedidaId { get; set; }
    }
}
