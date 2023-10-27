namespace LojaTobias.Api.Core.Models
{
    public class UsuarioModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public bool Removido { get; set; }
    }
}
