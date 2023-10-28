namespace LojaTobias.Core.Interfaces
{
    public interface ILogProvider
    {
        Task InserirLogInfo(string acao, string mensagem);
        Task InserirLogExcecao(string acao, string mensagem);
    }
}
