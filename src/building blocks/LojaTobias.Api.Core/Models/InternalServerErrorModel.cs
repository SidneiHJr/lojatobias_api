namespace LojaTobias.Api.Core.Models
{
    /// <summary>
    /// Modelo de retorno 500 (erro)
    /// </summary>
    public class InternalServerErrorModel
    {
        public InternalServerErrorModel(object resultado, ICollection<string> erros)
        {
            Resultado = resultado;
            Erros = erros;
        }

        public object Resultado { get; private set; }
        public ICollection<string> Erros { get; private set; }
    }
}