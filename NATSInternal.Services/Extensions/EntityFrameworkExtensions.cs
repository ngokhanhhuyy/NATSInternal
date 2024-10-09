namespace NATSInternal.Services.Extensions;

internal static class EntityFrameworkExtensions
{
    internal static ModelBuilder ConfigureEntity<T>(this ModelBuilder modelBuilder)
        where T : class, IEntity<T>, new()
    {
        return modelBuilder.Entity<T>(T.ConfigureModel);
    }

    internal static string GetStatsPropertyName<T, TUpdateHistory>(
            this IFinancialEngageableEntity<T, TUpdateHistory> _)
        where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
        where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    {
        Expression<Func<T, DateTime>> statsDateTimeExpression = T.StatsDateTimeExpression;
        if (statsDateTimeExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        // For cases where the property is cast to object (e.g., value types)
        if (statsDateTimeExpression.Body is UnaryExpression unaryExpression &&
            unaryExpression.Operand is MemberExpression operandExpression)
        {
            return operandExpression.Member.Name;
        }

        throw new ArgumentException("Invalid property expression.");
    }
}
