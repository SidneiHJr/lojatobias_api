namespace LojaTobias.Api.Core.Models
{
    public class PaginacaoListModel<T> : List<T>
    {
        public int NumeroPagina { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int Total { get; set; }

        public PaginacaoListModel(IEnumerable<T> items, int count, int numeroPagina, int tamnhoPagina)
        {
            NumeroPagina = numeroPagina;
            TotalPaginas = (int)Math.Ceiling(count / (double)tamnhoPagina);
            TamanhoPagina = tamnhoPagina;
            Total = count;
            this.AddRange(items);
        }

        public bool PreviousPage { get { return (NumeroPagina > 1); } }

        public bool NextPage { get { return (NumeroPagina < TotalPaginas); } }

        public static PaginacaoListModel<T> Create(IEnumerable<T> source, int numeroPagina, int tamanhoPagina)
        {
            var count = source.Count();
            var items = source.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina);
            return new PaginacaoListModel<T>(items, count, numeroPagina, tamanhoPagina);
        }

        public static PaginacaoListModel<T> Create(IQueryable<T> source, int numeroPagina, int tamanhoPagina)
        {
            var count = source.Count();
            var items = source.Skip((numeroPagina - 1) * tamanhoPagina).Take(tamanhoPagina);
            return new PaginacaoListModel<T>(items, count, numeroPagina, tamanhoPagina);
        }
    }
}
