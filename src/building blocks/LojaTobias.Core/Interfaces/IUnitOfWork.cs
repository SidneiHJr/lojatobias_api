namespace LojaTobias.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        Task<bool> CommitAsync();
        void BeginTransaction();
        bool CommitTransaction();
        Task<bool> CommitTransactionAsync();
        void RollBack();
        Task RollBackAsync();
        bool TransactionOpened();
    }
}
