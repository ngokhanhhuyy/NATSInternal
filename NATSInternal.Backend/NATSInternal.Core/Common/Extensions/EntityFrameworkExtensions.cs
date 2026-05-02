using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NATSInternal.Core.Common.Entities;

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

    extension<TEntity>(IQueryable<TEntity> query) where TEntity : IHasStatsEntity
    {
        public IQueryable<TEntity> HasStatsMonthYear(int year, int month)
        {
            return query.Where(e => e.StatsDateTime.Year == year && e.StatsDateTime.Month == month);
        }
    }
    #endregion
}
