using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Core.Entities
{
    public class Log : EntityBase
    {
        protected Log()
        {

        }

        public Log(string usuario, string acao, string tipo, string mensagem)
        {
            Usuario = usuario;
            Acao = acao;
            DataCriacao = DateTime.Now;
            Tipo = tipo;
            Mensagem = mensagem;
        }
        public string Usuario { get; private set; }
        public string Acao { get; private set; }
        public string Tipo { get; private set; }
        public string Mensagem { get; private set; }

    }
}
