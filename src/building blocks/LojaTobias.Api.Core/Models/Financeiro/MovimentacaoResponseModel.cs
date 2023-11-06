namespace LojaTobias.Api.Core.Models
{
    public class MovimentacaoResponseModel
    {
        public Guid Id { get; set; }
        public decimal Valor { get; set; }
        public Guid CaixaId { get; set; }
        public CaixaResponseModel Caixa { get; set; }
        public Guid? PedidoId { get; set; }
        public PedidoResponseModel? Pedido { get; set; }
        public Guid? AjusteId { get; set; }
        public AjusteResponseModel? Ajuste { get; set; }
        
    }
}
