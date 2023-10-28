using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class UsuarioModel
    {
        [Required(ErrorMessage = "Por favor informe o nome")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor informe o e-mail")]
        public required string Email { get; set; }
        public string? Perfil { get; set; }
        public bool Ativo { get; set; }
    }
}
