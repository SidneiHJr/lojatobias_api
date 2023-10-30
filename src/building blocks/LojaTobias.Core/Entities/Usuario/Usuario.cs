using LojaTobias.Core.Enums;
using LojaTobias.Core.ValueObjects;

namespace LojaTobias.Core.Entities
{
    public class Usuario : EntityBase
    {
        protected Usuario()
        {

        }

        public Usuario(Guid id, string nome, string email, string perfil)
        {
            Id = id;
            Nome = nome;
            Perfil = perfil;
            Email = new Email(email);
            Ativo = true;
            Removido = false;
        }


        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string Perfil { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }

        public IEnumerable<string> Validar()
        {
            var erros = new List<string>();

            if (!Email.EmailValido())
                erros.Add("Email inválido.");

            return erros;
        }

        public void Atualizar(Usuario usuario)
        {
            Nome = usuario.Nome;
            Email = usuario.Email;
            Ativo = usuario.Ativo;
        }

        public void Remover()
        {
            Removido = true;
        }
    }
}
