using LojaTobias.Core.Enums;

namespace LojaTobias.Core.Entities
{
    public class Movimentacao : EntityBase
    {
        protected Movimentacao()
        {
            
        }

        public Movimentacao(string categoria, string tipo)
        {
            Categoria = categoria;
            Tipo = tipo;  
        }

        public string Categoria { get; private set; } //Financeira, Estoque
        public string Tipo { get; private set; }
        public decimal? Valor { get; private set; }
        public decimal? Quantidade { get; private set; }

        public Guid? CaixaId { get; private set; }
        public virtual Caixa? Caixa { get; private set; }

        public Guid? ProdutoId { get; private set; }
        public virtual Produto? Produto { get; private set; }

        public Guid? PedidoId { get; private set; }
        public virtual Pedido? Pedido { get; private set; }

        public Guid? AjusteId { get; private set; }
        public virtual Ajuste? Ajuste { get; private set; }

        public void NovaMovimentacaoEstoque(Guid? pedidoId, Guid? ajusteId, Guid produtoId, decimal quantidade)
        {
            Quantidade = quantidade;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            AjusteId = ajusteId;
        }

        public void NovaMovimentacaoFinanceiro(Guid caixaId, Guid pedidoId, decimal valor)
        {
            CaixaId = caixaId;
            PedidoId = pedidoId;
            Valor = valor;
        }

        public IEnumerable<string> Validar()
        {
            var erros = new List<string>();

            if (Tipo != TipoMovimentacaoEnum.Credito.ToString() && Tipo != TipoMovimentacaoEnum.Debito.ToString())
            {
                erros.Add("O tipo da movimentação só pode ser Credito ou Debito");
            }

            return erros;
        }

    }
}
