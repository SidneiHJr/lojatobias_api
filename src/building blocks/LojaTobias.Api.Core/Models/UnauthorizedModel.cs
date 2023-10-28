namespace LojaTobias.Api.Core.Models
{
    public class UnauthorizedModel
    {
        public UnauthorizedModel(IEnumerable<object> erros)
        {
            Autenticado = false;
            Erros = erros;
        }

        public bool Autenticado { get; private set; } 
        public IEnumerable<object> Erros { get; private set; }
    }
}
