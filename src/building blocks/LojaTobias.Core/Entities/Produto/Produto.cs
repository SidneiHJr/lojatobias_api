﻿namespace LojaTobias.Core.Entities
{
    public class Produto : EntityBase
    {
        protected Produto()
        {
            
        }

        public string Nome { get; private set; }
        public bool Ativo { get; private set; }

        public ICollection<ProdutoUnidade> ProdutosUnidade { get; private set; }
    }
}
