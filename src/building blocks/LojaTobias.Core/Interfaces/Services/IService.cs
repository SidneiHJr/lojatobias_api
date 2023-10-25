using LojaTobias.Core.Entities;

namespace LojaTobias.Core.Interfaces
{
    public interface IService<TEntity> where TEntity : EntityBase
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity?> GetAsync(Guid id);
        Task<Guid> InsertAsync(TEntity entity);
        Task UpdateAsync(Guid id, TEntity entity);
        Task DeleteAsync(Guid id);
    }
}
