namespace LojaTobias.Api.Core.Models
{
    public class UsuarioResponseModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
    }
}
