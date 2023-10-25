using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        public readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public virtual async Task<IQueryable<TEntity>> GetAsync()
        {
            return await Task.FromResult(Table);
        }

        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }

        public virtual void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);

            if(entity != null)
                Table.Remove(entity);
        }
    }
}
