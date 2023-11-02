using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LojaTobias.Core.Entities
{
    public class Produto : EntityBase
    {
        protected Produto()
        {

        }

        public Produto(string nome, string descricao, Guid unidadeMedidaId)
        {
            Nome = nome;
            Descricao = descricao;
            UnidadeMedidaId = unidadeMedidaId;
            Ativo = true;
            Removido = false;
        }

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string Nome { get; private set; }

        [Column(TypeName = "varchar")]
        [StringLength(1000)]
        public string? Descricao { get; private set; }

        public decimal Quantidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }
        
        public Guid UnidadeMedidaId { get; private set; }
        public UnidadeMedida UnidadeMedida { get; private set; }

        public void Atualizar(Produto produto)
        {
            Nome = produto.Nome;
            Ativo = produto.Ativo;
        }

        public void Remover()
        {
            Removido = true;
        }
    }
}
