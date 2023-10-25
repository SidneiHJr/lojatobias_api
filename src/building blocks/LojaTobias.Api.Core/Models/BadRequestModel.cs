namespace LojaTobias.Api.Core.Models
{
    /// <summary>
    /// Modelo de retorno 400 (badRequest)
    /// </summary>
    public class BadRequestModel
    {
        public BadRequestModel(IEnumerable<object> erros)
        {
            Sucesso = false;
            Erros = erros;
        }

        public bool Sucesso { get; private set; }
        public IEnumerable<object> Erros { get; private set; }
    }
}