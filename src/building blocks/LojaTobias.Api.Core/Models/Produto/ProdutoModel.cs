using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class ProdutoModel
    {
        [Required(ErrorMessage = "Por favor informe um nome para o produto")]
        public required string Nome { get; set; }
    }
}
