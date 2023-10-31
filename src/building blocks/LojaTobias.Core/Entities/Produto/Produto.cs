namespace LojaTobias.Core.Entities
{
    public class Produto : EntityBase
    {
        protected Produto()
        {
            
        }

        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public Guid ProdutoUnidadeId { get; private set; }
        public ICollection<ProdutoUnidade> ProdutosUnidade { get; private set; }
    }

    public class ProdutoUnidade : EntityBase
    {
        protected ProdutoUnidade()
        {

        }
        public string Embalagem { get; private set; }
        public Guid ProdutoId { get; private set; }
        public Produto Produto{ get; private set; }
        public Guid UnidadeMedidaId{ get; private set; }
        public UnidadeMedida UnidadeMedida{ get; private set; }
    }

    public class UnidadeMedida : EntityBase
    {
        protected UnidadeMedida()
        {

        }

        public string Nome { get; private set; }
        public string Abreviacao { get; private set; }
    }
}
