namespace NATSInternal.Core.Extensions;

internal static class EntityExtensions
{
    #region ExtensionMethods
    public static bool IsLocked<T, TData>(this IHasStatsEntity<T, TData> entity)
        where T : class, IUpsertableEntity<T>
        where TData : class
    {
        DateTime lockedDate = entity.CreatedDateTime.AddMonths(2);
        DateTime lockedDateTime = new DateTime(lockedDate.Year, lockedDate.Month, 1, 0, 0, 0);
        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
        return currentDateTime >= lockedDateTime;
    }
    #endregion
}
