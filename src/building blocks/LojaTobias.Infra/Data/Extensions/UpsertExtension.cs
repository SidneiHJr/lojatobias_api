using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public static partial class ContextExtensions
    {
        public static SingleEntityUpsert<T> Upsert<T>(this DbContext context, T entity, T? originalValuesEntity) where T : EntityBase
        {
            var entry = context.Entry(entity);
            if (entry.State == (entry.State & (EntityState.Unchanged | EntityState.Detached)))
                entry.State = entity.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
            if (entry.State == EntityState.Modified && originalValuesEntity != null)
                entry.OriginalValues.SetValues(originalValuesEntity);
            return new SingleEntityUpsert<T>(context, entity);
        }

        public static SingleEntityUpsert<T> Upsert<T>(this DbContext context, T entity) where T : EntityBase
        {
            var entry = context.Entry(entity);
            if (entry.State == (entry.State & (EntityState.Unchanged | EntityState.Detached)))
                entry.State = entity.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
            return new SingleEntityUpsert<T>(context, entity);
        }

        public static SingleEntityUpsert<U> Upsert<T, U>(this SingleEntityUpsert<T, U> upsertable, U entity)
            where T : EntityBase
            where U : EntityBase
        {
            return new SingleEntityUpsert<U>(upsertable.Context, entity);
        }

        public static SingleEntityUpsert<U> Upsert<T, U>(this ManyEntitiesUpsert<T> upsertable, U entity)
            where T : EntityBase
            where U : EntityBase
        {
            return new SingleEntityUpsert<U>(upsertable.Context, entity);
        }

        public static SingleEntityUpsert<U, T> ThenUpsert<T, U>(this SingleEntityUpsert<T> upsertable, Func<T, U> func)
            where T : EntityBase
            where U : EntityBase
        {
            var entity = func.Invoke(upsertable.Entity);
            upsertable.Context.Upsert(entity);
            return new SingleEntityUpsert<U, T>(upsertable.Context, upsertable.Entity, entity);
        }

        public static ManyEntitiesUpsert<T> UpsertMany<T>(this DbContext context, IEnumerable<T> entities)
            where T : EntityBase
        {
            foreach (var entity in entities)
                context.Upsert(entity);
            return new ManyEntitiesUpsert<T>(context, entities);
        }

        public static ManyEntitiesUpsert<U> UpsertMany<T, U>(this ManyEntitiesUpsert<T> upsertable, IEnumerable<U> entities)
            where T : EntityBase
            where U : EntityBase
        {
            return new ManyEntitiesUpsert<U>(upsertable.Context, entities);
        }

        public static ManyEntitiesUpsert<U> ThenUpsertMany<T, U>(this SingleEntityUpsert<T> upsertable, Func<T, IEnumerable<U>> func)
            where T : EntityBase
            where U : EntityBase
        {
            var entities = func.Invoke(upsertable.Entity);
            foreach (var entity in entities)
                upsertable.Context.Upsert(entity);

            return new ManyEntitiesUpsert<U>(upsertable.Context, entities);
        }

        public static ManyEntitiesUpsert<U> ThenUpsertMany<T, U>(this ManyEntitiesUpsert<T> upsertable, Func<T, IEnumerable<U>> func)
            where T : EntityBase
            where U : EntityBase
        {
            var entities = upsertable.Entities.SelectMany(p => func.Invoke(p));
            foreach (var child in entities)
                upsertable.Context.Upsert(child);

            return new ManyEntitiesUpsert<U>(upsertable.Context, entities);
        }

        public static ManyEntitiesUpsert<U> ThenUpsert<T, U>(this ManyEntitiesUpsert<T> upsertable, Func<T, U> func)
            where T : EntityBase
            where U : EntityBase
        {
            var entities = upsertable.Entities.Select(p => func.Invoke(p));
            foreach (var child in entities)
                upsertable.Context.Upsert(child);

            return new ManyEntitiesUpsert<U>(upsertable.Context, entities);
        }
    }

    public class SingleEntityUpsert<T>
        where T : EntityBase
    {
        internal DbContext Context { get; set; }
        internal T Entity { get; set; }

        internal SingleEntityUpsert(DbContext context, T entity)
        {
            this.Context = context;
            this.Entity = entity;
        }
    }

    public class SingleEntityUpsert<T, TOriginal> : SingleEntityUpsert<T>
        where T : EntityBase
    {
        public TOriginal Original { get; set; }

        public SingleEntityUpsert(DbContext context, TOriginal original, T entity)
            : base(context, entity)
        {
            this.Original = original;
        }
    }

    public class ManyEntitiesUpsert<T>
    {
        public DbContext Context { get; set; }
        public IEnumerable<T> Entities { get; set; }
        public ManyEntitiesUpsert(DbContext context, IEnumerable<T> entities)
        {
            this.Context = context;
            this.Entities = entities;
        }
    }
}
