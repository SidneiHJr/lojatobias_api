using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class UnidadeMedidaModel
    {
        [Required(ErrorMessage = "Por favor informe o nome")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor informe a abreviacao")]
        public required string Abreviacao { get; set; }
    }
}
