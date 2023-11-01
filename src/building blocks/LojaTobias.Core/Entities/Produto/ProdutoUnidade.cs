namespace LojaTobias.Core.Entities
{
    public class ProdutoUnidade : EntityBase
    {
        protected ProdutoUnidade()
        {

        }
        public decimal Quantidade { get; private set; }
        public string? Embalagem { get; private set; }
        public decimal? PesoEmbalagem { get; private set; }

        public Guid ProdutoId { get; private set; }
        public Produto Produto{ get; private set; }

        public Guid UnidadeMedidaId{ get; private set; }
        public UnidadeMedida UnidadeMedida{ get; private set; }
    }
}
