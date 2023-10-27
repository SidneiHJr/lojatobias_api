using LojaTobias.Core.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LojaTobias.Core.Entities
{
    public class Usuario : EntityBase
    {
        protected Usuario()
        {

        }

        public Usuario(Guid id, string nome, string email)
        {
            Id = id;
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
