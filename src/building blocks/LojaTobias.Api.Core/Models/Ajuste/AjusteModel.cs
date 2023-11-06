
namespace LojaTobias.Api.Core.Models
{
    public class AjusteModel
    {
        public string Tipo { get; set; }
        public string Motivo { get; set; }
        public decimal Quantidade { get; set; }
        public Guid UnidadeMedidaId { get; set; }
        public Guid ProdutoId { get; set; }
    }
}
