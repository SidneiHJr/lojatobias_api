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
        public string Descricao { get; private set; }
        public decimal Quantidade { get; private set; }
        public bool Ativo { get; private set; }
        public bool Removido { get; private set; }
        
        public Guid UnidadeMedidaId { get; private set; }
        public UnidadeMedida UnidadeMedida { get; private set; }

        public void Atualizar(Produto produto)
        {
            Nome = produto.Nome;
            Ativo = produto.Ativo;
        }

        public void Remover()
        {
            Removido = true;
        }
    }
}
