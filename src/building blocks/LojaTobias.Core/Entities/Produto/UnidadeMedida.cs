using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Core.Entities
{
    public class UnidadeMedida : EntityBase
    {
        protected UnidadeMedida()
        {

        }

        public string Nome { get; private set; }
        public string Abreviacao { get; private set; }
        public bool Removido { get; private set; }

        public virtual ICollection<UnidadeMedidaConversao> ConversoesEntrada { get; set; }
        public virtual ICollection<UnidadeMedidaConversao> ConversoesSaida { get; set; }

        public void Atualizar(UnidadeMedida unidadeMedida)
        {
            Nome = unidadeMedida.Nome;
            Abreviacao = unidadeMedida.Abreviacao;
        }

        public void Remover()
        {
            Removido = true;
        }

    }
}
