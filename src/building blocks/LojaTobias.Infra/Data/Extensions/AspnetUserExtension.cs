using LojaTobias.Core.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace LojaTobias.Infra.Extensions
{
    public class AspnetUserExtension : IdentityUser
    {
        protected AspnetUserExtension()
        {
            Active = true;
        }

        public AspnetUserExtension(string email, string nome) : base(email)
        {
            Name = nome;
            Email = email;
            Active = true;
        }

        public AspnetUserExtension(string email, string nome, bool active) : base(email)
        {
            Name = nome;
            Email = email;
            Active = active;
        }

        public string Name { get; private set; }
        public bool Active { get; private set; }

        public void Atualizar(string name, string email, bool active = true)
        {
            Name = name;
            Email = email;
            NormalizedEmail = email.ToUpper();
            NormalizedUserName = email.ToUpper();
            Active = active;
        }

    }
}
