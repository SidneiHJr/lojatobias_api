using System.Net.Mail;

namespace LojaTobias.Core.ValueObjects
{
    public class Email
    {
        public Email()
        {
                
        }

        public Email(string endereco)
        {
            Endereco = endereco;
        }

        public string Endereco { get; set; }

        public bool EmailValido()
        {
            if (!MailAddress.TryCreate(Endereco, out var mailAddress))
                return false;

            var hostParts = mailAddress.Host.Split('.');

            if (hostParts.Length == 1)
                return false; // Sem '.'

            if (hostParts.Any(p => p == string.Empty))
                return false; // Duplo '.'

            if (hostParts[^1].Length < 2)
                return false; // TLD única letra.

            if (mailAddress.User.Contains(' '))
                return false;

            if (mailAddress.User.Split('.').Any(p => p == string.Empty))
                return false; // Duplo '.' ou '.' no final.

            return true;
        }
    }
}
