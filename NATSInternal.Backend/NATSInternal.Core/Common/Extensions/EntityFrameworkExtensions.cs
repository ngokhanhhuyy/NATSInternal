using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NATSInternal.Core.Common.Extensions;

internal static class EntityFrameworkExtensions
{
    #region ExtensionMethods
    extension<TData>(PropertyBuilder<TData> propertyBuilder) where TData : class
    {
        public PropertyBuilder<TData> HasJsonConversion(JsonSerializerOptions serializerOptions)
        {
            return propertyBuilder.HasConversion(
                data => JsonSerializer.Serialize(data, serializerOptions),
                json => JsonSerializer.Deserialize<TData>(json, serializerOptions)!
            );
        }
    }

    extension<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        public IOrderedQueryable<TEntity> ApplySorting(
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending)
        {
            return sortByAscending ? query.OrderBy(propertySelector) : query.OrderByDescending(propertySelector);
        }
    }

    extension<TEntity>(IOrderedQueryable<TEntity> query) where TEntity : class
    {
        public IOrderedQueryable<TEntity> ThenApplySorting(
            Expression<Func<TEntity, object?>> propertySelector,
            bool sortByAscending)
        {
            return sortByAscending
                ? query.ThenBy(propertySelector)
                : query.ThenByDescending(propertySelector);
        }
    }
    #endregion
}
