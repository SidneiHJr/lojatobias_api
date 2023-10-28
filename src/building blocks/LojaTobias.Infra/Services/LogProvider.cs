using LojaTobias.Core.Entities;
using LojaTobias.Core.Enums;
using LojaTobias.Core.Interfaces;

namespace LojaTobias.Infra.Services
{
    public class LogProvider : ILogProvider
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Log> _logRepository;
        private readonly IAspnetUser _aspnetUser;

        public LogProvider(IUnitOfWork unitOfWork, IRepository<Log> logRepository, IAspnetUser aspnetUser)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _aspnetUser = aspnetUser;
        }

        public async Task InserirLogInfo(string acao, string mensagem)
        {
            var log = new Log(BuscarUsuario(), acao, TipoLogEnum.Info.ToString(), mensagem);

            await _logRepository.InsertAsync(log);

            await _unitOfWork.CommitAsync();
        }

        public async Task InserirLogExcecao(string acao, string mensagem)
        {
            var log = new Log(BuscarUsuario(), acao, TipoLogEnum.Excecao.ToString(), mensagem);

            await _logRepository.InsertAsync(log);

            await _unitOfWork.CommitAsync();
        }


        private string BuscarUsuario() => string.IsNullOrEmpty(_aspnetUser.GetUserEmail()) ? "Sistema" : _aspnetUser.GetUserEmail();
    }
}
