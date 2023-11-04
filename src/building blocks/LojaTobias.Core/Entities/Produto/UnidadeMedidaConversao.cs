namespace LojaTobias.Core.Entities
{
    public class UnidadeMedidaConversao : EntityBase
    {
        protected UnidadeMedidaConversao()
        {
                
        }


        public decimal FatorConversao { get; private set; } 
        public Guid UnidadeMedidaEntradaId { get; private set; }
        public virtual UnidadeMedida UnidadeMedidaEntrada { get; private set; }
        public Guid UnidadeMedidaSaidaId { get; private set; }
        public virtual UnidadeMedida UnidadeMedidaSaida { get; private set; }

        public IEnumerable<string> Validar()
        {
            var erros = new List<string>();

            if (UnidadeMedidaEntradaId == UnidadeMedidaSaidaId)
            {
                erros.Add("A unidade de medida de entrada é a mesma de saida. Não é necessário uma conversão");
            }

            return erros;
        }
    }

    
}
