using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NATSInternal.Infrastructure.Extensions;

internal static class EntityFrameworkExtensions
{
    #region ExtensionMethods
    public static PropertyBuilder<TData> HasJsonConversion<TData>(
            this PropertyBuilder<TData> propertyBuilder,
            JsonSerializerOptions serializerOptions)
        where TData : class
    {
        return propertyBuilder.HasConversion(
            data => JsonSerializer.Serialize(data, serializerOptions),
            json => JsonSerializer.Deserialize<TData>(json, serializerOptions)!
        );
    }

    public static IOrderedQueryable<TEntity> ApplySorting<TEntity>(
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending)
    {
        return sortByAscending
            ? query.OrderBy(propertySelector)
            : query.OrderByDescending(propertySelector);
    }

    public static IOrderedQueryable<TEntity> ThenApplySorting<TEntity>(
            this IOrderedQueryable<TEntity> query,
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending)
    {
        return sortByAscending
            ? query.ThenBy(propertySelector)
            : query.ThenByDescending(propertySelector);
    }
    #endregion
}
