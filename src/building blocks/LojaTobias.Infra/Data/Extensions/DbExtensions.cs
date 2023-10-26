using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LojaTobias.Infra.Data
{
    public static class DbExtensions
    {
        public static void Properties<T>(this ModelBuilder builder, Action<PropertyBuilder<T>> callback)
        {
            var types = builder.Model.GetEntityTypes();
            foreach (var type in types)
            {
                var entityBuilderType = typeof(EntityTypeBuilder<>).MakeGenericType(type.ClrType);

                var buildEntity = typeof(ModelBuilder).GetMethods()
                    .Single(m => m.Name == "Entity" && m.GetGenericArguments().Any() && !m.GetParameters().Any())
                    .MakeGenericMethod(type.ClrType);

                var buildProperty = entityBuilderType.GetMethods()
                    .Single(m => m.Name == "Property" && m.GetGenericArguments().Any() && m.GetParameters().Any(p => p.ParameterType == typeof(string)))
                    .MakeGenericMethod(type.ClrType);

                var entityTypeBuilder = buildEntity.Invoke(builder, null);
                var properties = type.GetProperties().Where(x => x.GetType().Equals(typeof(T)));
                foreach (var property in properties)
                {
                    var propertyBuilder = buildProperty.Invoke(entityTypeBuilder, new[] { property.Name }) as PropertyBuilder<T>;
                    callback(propertyBuilder);
                }
            }
        }
    }
}
