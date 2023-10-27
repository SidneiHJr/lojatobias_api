using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Api.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Por favor informe o e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor informe a senha")]
        public string Senha { get; set; }
    }
}
