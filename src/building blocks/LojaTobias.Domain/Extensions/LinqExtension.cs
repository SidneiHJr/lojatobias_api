using System.Linq.Expressions;

namespace LojaTobias.Domain.Extensions
{
    public static class LinqExtension
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnName, string direcao)
        {
            if (String.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(direcao))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = direcao == "asc" ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                              new Type[] { source.ElementType, property.Type },
                                                              source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }

        public static List<T> OrderBy<T>(this List<T> source, string columnName, string direcao)
        {
            if (string.IsNullOrEmpty(direcao))
                return source;

            var propertyInfo = typeof(T).GetProperty(columnName);

            if (direcao == "asc")
            {
                return source.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
            }
            else
            {
                return source.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
            }
        }

        public static IQueryable<T> OrderByProp<T>(this IQueryable<T> source, Expression<Func<T, string>> function, string direcao)
        {
            if (string.IsNullOrEmpty(direcao))
                return source;

            if (direcao == "asc")
            {
                return source.OrderBy(function);
            }
            else
            {
                return source.OrderByDescending(function);
            }
        }

        public static IQueryable<T> OrderByProp<T>(this IQueryable<T> source, Expression<Func<T, DateTime>> function, string direcao)
        {
            if (string.IsNullOrEmpty(direcao))
                return source;

            if (direcao == "asc")
            {
                return source.OrderBy(function);
            }
            else
            {
                return source.OrderByDescending(function);
            }
        }
    }
}
