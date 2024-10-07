namespace NATSInternal.Services;

internal abstract class AbstractFinancialEngagementService<T, TUser, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUser, TUpdateHistory>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    protected abstract DatabaseContext Context { get; init; }
    protected abstract DbSet<T> Repository { get; init; }
    protected abstract IMonthYearService<T, TUser, TUpdateHistory> MonthYearService { get; init; }
}