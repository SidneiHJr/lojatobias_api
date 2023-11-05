namespace LojaTobias.Api.Core.Models
{
    public class PedidoFiltroModel : FiltroModelBase
    {
        public string? Termo { get; set; }
        public string Tipo { get; set; }
        public string? Status { get; set; }
    }
}
