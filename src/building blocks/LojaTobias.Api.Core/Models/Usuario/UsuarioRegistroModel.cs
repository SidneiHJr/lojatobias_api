using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class UsuarioRegistroModel
    {
        [Required(ErrorMessage = "Por favor informe o nome do usuário")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor informe o e-mail")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Por favor informe o perfil do usuário")]
        public required string Perfil { get; set; }

        [Required(ErrorMessage = "Por favor informe a senha")]
        [StringLength(100, ErrorMessage = "A senha precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public required string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public required string ConfirmacaoSenha { get; set; }
    }
}
