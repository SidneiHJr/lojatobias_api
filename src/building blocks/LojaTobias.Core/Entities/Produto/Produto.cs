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

        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
        public decimal Quantidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }
        
        public Guid UnidadeMedidaId { get; private set; }
        public virtual UnidadeMedida UnidadeMedida { get; private set; }

        public virtual ICollection<PedidoItem> Pedidos { get; private set; }
        public virtual ICollection<Movimentacao> Movimentacoes { get; private set; }
        public virtual ICollection<Ajuste> Ajustes { get; private set; }

        public void Atualizar(Produto produto)
        {
            Nome = produto.Nome;
            Ativo = produto.Ativo;
        }

        public void Remover()
        {
            Removido = true;
        }

        public void AdicionarQuantidade(decimal quantidade)
        {
            Quantidade += quantidade;
        }

        public void RemoverQuantidade(decimal quantidade)
        {
            Quantidade -= quantidade;
        }
    }
}
