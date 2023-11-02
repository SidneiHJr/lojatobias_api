
namespace LojaTobias.Api.Core.Models
{
    public class ProdutoResponseModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Quantidade { get; set; }
        public UnidadeMedidaResponseModel UnidadeMedida { get; set; }
    }
}
