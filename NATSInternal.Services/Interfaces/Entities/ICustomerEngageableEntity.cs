namespace NATSInternal.Services.Interfaces.Entities;

internal interface ICustomerEngageableEntity<T, TCustomer, TUser, TUpdateHistory>
    : IFinancialEngageableEntity<T, TUser, TUpdateHistory>
    where T : class, IUpsertableEntity<T>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    int CustomerId { get; set; }
    TCustomer Customer { get; set; }
}