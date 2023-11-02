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

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string Nome { get; private set; }

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string Abreviacao { get; private set; }

        public bool Removido { get; private set; }

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
