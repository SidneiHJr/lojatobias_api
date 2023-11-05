using LojaTobias.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public static partial class ContextExtensions
    {
        public static ManyEntitiesUpsert<T> UpdateMany<T>(
            this DbContext context,
            IEnumerable<T> source,
            IEnumerable<T> target,
            IEqualityComparer<T> comparer
        )
            where T : EntityBase
        {
            var originalValuesEntities = new Dictionary<Guid, T>();

            var deleted = source.Except(target, comparer);
            foreach (var delete in deleted)
                context.Entry(delete).State = EntityState.Deleted;

            var updated = target.Intersect(source, comparer);
            foreach (var item in updated)
            {
                var original = source.First(p => comparer.Equals(item, p));
                originalValuesEntities.Add(original.Id, original);
                item.Id = original.Id;
            }

            foreach (var entity in target)
            {
                T? original = null;
                originalValuesEntities.TryGetValue(entity.Id, out original);
                context.Upsert(entity, original);
            }

            return new ManyEntitiesUpsert<T>(context, target);
        }

        public static ManyEntitiesUpsert<T> UpdateMany<T>(
            this DbContext context,
            IEnumerable<T> source,
            IEnumerable<T> target
        )
            where T : EntityBase
        {
            return context.UpdateMany(source, target, new EntityComparer<T>());
        }

        public static ManyEntitiesUpsert<U> UpdateMany<T, U>(this ManyEntitiesUpsert<T> upsertable, IEnumerable<U> entities)
            where T : EntityBase
            where U : EntityBase
        {
            return new ManyEntitiesUpsert<U>(upsertable.Context, entities);
        }

        public static ManyEntitiesUpsert<U> ThenUpdateMany<T, U>(this SingleEntityUpsert<T> upsertable, IEnumerable<U> source, Func<T, IEnumerable<U>> targetFunc)
            where T : EntityBase
            where U : EntityBase
        {
            var context = upsertable.Context;
            var entity = upsertable.Entity;
            var target = targetFunc.Invoke(entity);

            return context.UpdateMany(source, target, new EntityComparer<U>()); ;
        }

        public static ManyEntitiesUpsert<U> ThenUpdateMany<T, U>(
            this SingleEntityUpsert<T> upsertable,
            IEnumerable<U> source,
            Func<T, IEnumerable<U>> targetFunc,
            IEqualityComparer<U> comparer
        )
            where T : EntityBase
            where U : EntityBase
        {
            var context = upsertable.Context;
            var entity = upsertable.Entity;
            var target = targetFunc.Invoke(entity);

            return context.UpdateMany(source, target, comparer);
        }
        public static ManyEntitiesUpsert<U> ThenUpdateMany<T, U>(
            this SingleEntityUpsert<T> upsertable,
            IEnumerable<U> source,
            IEnumerable<U> target,
            IEqualityComparer<U> comparer
        )
            where T : EntityBase
            where U : EntityBase
        {
            var context = upsertable.Context;
            var entity = upsertable.Entity;

            return context.UpdateMany(source, target, comparer);
        }

        public static ManyEntitiesUpsert<U> ThenUpdateMany<T, U>(
            this SingleEntityUpsert<T> upsertable,
            IEnumerable<U> source,
            IEnumerable<U> target
        )
            where T : EntityBase
            where U : EntityBase
        {
            var context = upsertable.Context;
            var entity = upsertable.Entity;
            var comparer = new EntityComparer<U>();

            return context.UpdateMany(source, target, comparer);
        }

        public static ManyEntitiesUpsert<U> ThenUpdateMany<T, U>(
            this ManyEntitiesUpsert<T> upsertable,
            IEnumerable<U> source,
            Func<T, IEnumerable<U>> targetFunc,
            IEqualityComparer<U> comparer
        )
            where T : EntityBase
            where U : EntityBase
        {
            var context = upsertable.Context;
            var entities = upsertable.Entities;
            var result = new List<U>();
            var target = entities.SelectMany(p => targetFunc.Invoke(p));

            return context.UpdateMany(source, target, comparer);
        }

        public static ManyEntitiesUpsert<U> ThenUpdateMany<T, U>(this ManyEntitiesUpsert<T> upsertable, IEnumerable<U> source, Func<T, IEnumerable<U>> targetFunc)
            where T : EntityBase
            where U : EntityBase
        {
            var context = upsertable.Context;
            var entities = upsertable.Entities;
            var result = new List<U>();
            var target = entities.SelectMany(p => targetFunc.Invoke(p));
            var comparer = new EntityComparer<U>();

            return context.UpdateMany(source, target, comparer);
        }
    }
}
