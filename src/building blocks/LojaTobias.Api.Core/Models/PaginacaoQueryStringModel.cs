namespace LojaTobias.Api.Core.Models
{
    public class PaginacaoQueryStringModel
    {
        const int tamanhoMaximoPagina = 10000;

        /// <summary>
        /// Número da página a ser exíbida
        /// </summary>
        public int NumeroPagina { get; set; } = 1;

        private int _tamanhoPagina = 10;

        /// <summary>
        /// Tamanho da página a ser exíbida
        /// </summary>
        public int TamanhoPagina
        {
            get
            {
                return _tamanhoPagina;
            }
            set
            {
                _tamanhoPagina = (value > tamanhoMaximoPagina) ? tamanhoMaximoPagina : value;
            }
        }
    }
}
