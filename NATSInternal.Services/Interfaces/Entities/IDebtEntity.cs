namespace NATSInternal.Services.Interfaces.Entities;

internal interface IDebtEntity<T, TUpdateHistory>
    : ICustomerEngageableEntity<T, TUpdateHistory>
    where T : class, ICustomerEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    long Amount { get; set; }
}