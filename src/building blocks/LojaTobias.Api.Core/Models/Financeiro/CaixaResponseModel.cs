namespace LojaTobias.Api.Core.Models
{
    public class CaixaResponseModel
    {
        public Guid Id { get; set; }
        public decimal Saldo { get; set; }
        public IEnumerable<MovimentacaoResponseModel> Movimentacoes { get; set; }
    }
}
