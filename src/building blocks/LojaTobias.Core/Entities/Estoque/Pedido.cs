using LojaTobias.Core.Enums;

namespace LojaTobias.Core.Entities
{
    public class Pedido : EntityBase
    {
        protected Pedido()
        {
                
        }

        public Pedido(string tipo, decimal total, string observacao, string fornecedor, string cliente, List<PedidoItem> produtos)
        {
            Tipo = tipo;
            Total = total;
            Observacao = observacao;
            Fornecedor = fornecedor;
            Cliente = cliente;
            Produtos = produtos;
            Status = StatusPedidoEnum.Realizado.ToString();
        }

        public string Tipo { get; private set; }
        public string? Observacao { get; private set; }
        public decimal Total { get; private set; }
        public string? Fornecedor { get; private set; }
        public string? Cliente { get; private set; }
        public string Status { get; private set; }

        public virtual ICollection<PedidoItem> Produtos { get; private set; }
        public virtual ICollection<Movimentacao> Movimentacoes { get; private set; }

        public void Atualizar(Pedido pedido)
        {
            Observacao = pedido.Observacao;
            Fornecedor = pedido.Fornecedor;
            Cliente = pedido.Cliente;
            Produtos = pedido.Produtos;
        }

        public IEnumerable<string> Validar()
        {
            var erros = new List<string>();

            if (Tipo == TipoPedidoEnum.Compra.ToString())
            {
                if(string.IsNullOrEmpty(Fornecedor))
                {
                    erros.Add("Informe o fornecedor");
                }
            }

            if(Tipo == TipoPedidoEnum.Venda.ToString())
            {
                if (string.IsNullOrEmpty(Cliente))
                {
                    erros.Add("informe o cliente");
                }
            }

            if(Total == 0)
            {
                erros.Add("O valor do pedido não pode ser zero");
            }

            return erros;
        }

        public void NovoPedido(string tipo)
        {
            Tipo = tipo;
            Status = StatusPedidoEnum.Realizado.ToString();
        }

        public void AtualizarTotal(decimal total)
        {
            Total = total;
        }

        public void AtualizarStatus(string status)
        {
            Status = status;
        }
    }
}
