namespace LojaTobias.Api.Core.Models
{
    /// <summary>
    /// Modelo de retorno 200 (sucesso)
    /// </summary>
    public class OkModel
    {
        public OkModel(object resultado)
        {
            Sucesso = true;
            Resultado = resultado;
        }
        
        public bool Sucesso { get; private set; }
        public object Resultado { get; private set; }
    }
}