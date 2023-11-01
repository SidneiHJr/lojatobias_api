namespace LojaTobias.Core.Entities
{
    public class UnidadeMedida : EntityBase
    {
        protected UnidadeMedida()
        {

        }

        public string Nome { get; private set; }
        public string Abreviacao { get; private set; }

        public ICollection<ProdutoUnidade> ProdutosUnidade { get; private set; }
    }
}
