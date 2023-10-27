using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class UsuarioRegistroModel
    {
        [Required(ErrorMessage = "Por favor informe o nome do usuário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor informe o e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor informe a senha")]
        [StringLength(100, ErrorMessage = "A senha precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmacaoSenha { get; set; }
    }
}
