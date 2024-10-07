namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICostEntity<T, TUser, TUpdateHistory>
    : IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUser, TUpdateHistory>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    long Amount { get; set; }
}
