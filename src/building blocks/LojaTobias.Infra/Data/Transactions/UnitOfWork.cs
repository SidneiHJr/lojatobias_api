
using LojaTobias.Core.Interfaces;

namespace LojaTobias.Infra.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        private readonly INotifiable _notifiable;

        public UnitOfWork(Context context, INotifiable notifiable)
        {
            _context = context;
            _notifiable = notifiable;
        }

        public bool Commit()
        {
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _notifiable.AddNotification("DbError", ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return false;
            }
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _notifiable.AddNotification("DbError", ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return false;
            }
        }

        public void BeginTransaction()
        {
            if (!TransactionOpened())
                _context.Database.BeginTransaction();
        }

        public async Task<bool> CommitTransactionAsync()
        {
            try
            {
                await _context.Database.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                _notifiable.AddNotification("DbError", ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return false;
            }
        }

        public bool CommitTransaction()
        {
            try
            {
                _context.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _notifiable.AddNotification("DbError", ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return false;
            }
        }

        public async Task RollBackAsync()
        {
            try
            {
                await _context.Database.RollbackTransactionAsync();

            }
            catch (Exception ex)
            {
                _notifiable.AddNotification("DbError", ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        public void RollBack()
        {
            try
            {
                _context.Database.RollbackTransaction();

            }
            catch (Exception ex)
            {
                _notifiable.AddNotification("DbError", ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        public bool TransactionOpened()
        {
            return _context.Database.CurrentTransaction != null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
