
using LojaTobias.Core.Entities;
using LojaTobias.Core.Interfaces;

namespace LojaTobias.Domain.Services
{
    public class Service<TEntity> : IService<TEntity> where TEntity : EntityBase
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly INotifiable _notifiable;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IAspnetUser _aspnetUser;

        public Service(IRepository<TEntity> repository, INotifiable notifiable, IUnitOfWork unitOfWork, IAspnetUser aspnetUser)
        {
            _repository = repository;
            _notifiable = notifiable;
            _unitOfWork = unitOfWork;
            _aspnetUser = aspnetUser;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _repository.GetAsync();
        }

        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            return await _repository.GetAsync(id);
        }

        public virtual async Task<Guid> InsertAsync(TEntity entity)
        {
            await Validate(entity);

            if (_notifiable.HasNotification)
                return Guid.Empty;

            entity.NovaInsercao(string.IsNullOrEmpty(entity.UsuarioCriacao) ? _aspnetUser.GetUserId() : entity.UsuarioCriacao);

            await _repository.InsertAsync(entity);
            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
            {
                _notifiable.AddNotification("Inserir", "Erro ao inserir o registro");
                return Guid.Empty;
            }

            return entity.Id;
        }

        public virtual async Task UpdateAsync(Guid id, TEntity entity)
        {
            await Validate(entity);

            if (_notifiable.HasNotification)
                return;

            var entityDb = await _repository.GetAsync(id);

            var method = entityDb.GetType().GetMethod("Atualizar");

            if (method != null)
                method.Invoke(entityDb, new object[] { entity });
            else
                return;

            entityDb.NovaAtualizacao(_aspnetUser.GetUserId());

            _repository.Update(entityDb);
            await _unitOfWork.CommitAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entityDb = await _repository.GetAsync(id);

            var method = entityDb.GetType().GetMethod("Remover");

            if (method != null)
            {
                method.Invoke(entityDb, null);
                entityDb.NovaAtualizacao(_aspnetUser.GetUserId());

                _repository.Update(entityDb);
            }
            else
                await _repository.DeleteAsync(id);

            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
                _notifiable.AddNotification("Deletar", $"Erro ao excluir o registro {id}");

            await Task.CompletedTask;
        }
        protected async Task Validate(TEntity item)
        {
            var method = item.GetType().GetMethod("Validar");

            if (method != null)
            {
                var erros = method.Invoke(item, null);
                _notifiable.AddNotifications(erros as IEnumerable<string>);
            }

            await Task.CompletedTask;
        }

    }
}
