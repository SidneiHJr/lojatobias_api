namespace LojaTobias.Core.Entities
{
    public class Produto : EntityBase
    {
        protected Produto()
        {

        }

        public Produto(string nome)
        {
            Nome = nome;
            Ativo = true;
            Removido = false;
        }

        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; } 
        public ICollection<ProdutoUnidade>? ProdutosUnidade { get; private set; }

        public void Remover()
        {
            Removido = true;
        }
    }
}
