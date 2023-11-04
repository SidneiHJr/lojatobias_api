namespace LojaTobias.Api.Core.Models
{
    public class UnidadeMedidaConversaoModel
    {
        public Guid UnidadeMedidaEntradaId { get; set; }
        public Guid UnidadeMedidaSaidaId { get; set; }
        public decimal FatorConversao { get; set; }
    }
}
