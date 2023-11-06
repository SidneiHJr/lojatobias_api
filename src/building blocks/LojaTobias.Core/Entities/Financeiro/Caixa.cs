
namespace LojaTobias.Core.Entities
{
    public class Caixa : EntityBase
    {
        protected Caixa()
        {

        }
        public Caixa(decimal saldoInicial)
        {
            Saldo = saldoInicial;
        }

        public decimal Saldo { get; private set; }
        public virtual ICollection<Movimentacao> Movimentacoes { get; private set; }

        public void AdicionarSaldo(decimal valor)
        {
            Saldo += valor;
        }

        public void SubtrairSaldo(decimal valor)
        {
            Saldo -= valor;
        }

    }
}
