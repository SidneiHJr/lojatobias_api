using LojaTobias.Core.ValueObjects;

namespace LojaTobias.Core.Entities
{
    public class Usuario : EntityBase
    {
        protected Usuario()
        {

        }

        public Usuario(string nome, string email)
        {
            Nome = nome;
            Email = new Email(email);
            Ativo = true;
            Removido = false;
        }


        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }

        public void Remover()
        {
            Removido = true;
        }
    }
}
