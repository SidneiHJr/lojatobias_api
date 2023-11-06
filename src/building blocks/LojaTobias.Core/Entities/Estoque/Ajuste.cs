using LojaTobias.Core.Enums;

namespace LojaTobias.Core.Entities
{
    public class Ajuste : EntityBase
    {
        protected Ajuste()
        {

        }

        public string Tipo { get; private set; }
        public string Motivo { get; private set; }
        public decimal Quantidade { get; private set; }

        public Guid UnidadeMedidaId { get; private set; }
        public virtual UnidadeMedida UnidadeMedida { get; private set; }

        public Guid ProdutoId { get; private set; }
        public virtual Produto Produto { get; private set; }

        public Guid MovimentacaoId { get; private set; }
        public virtual Movimentacao Movimentacao { get; private set; }

        public IEnumerable<string> Validar()
        {
            var erros = new List<string>();

            if(Tipo != TipoAjusteEnum.Debito.ToString() && Tipo != TipoAjusteEnum.Credito.ToString())
            {
                erros.Add("Tipo deve ser Debito ou Credito");
            }

            return erros;
        }

    }
}
