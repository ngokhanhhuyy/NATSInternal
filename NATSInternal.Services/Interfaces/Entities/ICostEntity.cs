namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICostEntity<T, TUser, TUpdateHistory>
    : IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    where TUser : class, IUserEntity<TUser>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>
{
    long Amount { get; set; }
}
