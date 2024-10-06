namespace NATSInternal.Services.Interfaces.Entities;

internal interface IDebtEntity<T, TCustomer, TUser, TUpdateHistory>
    : ICustomerEngageableEntity<T, TCustomer, TUser, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUser, TUpdateHistory>, new()
    where TCustomer : class, ICustomerEntity<TCustomer, TUser>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    long Amount { get; set; }
}