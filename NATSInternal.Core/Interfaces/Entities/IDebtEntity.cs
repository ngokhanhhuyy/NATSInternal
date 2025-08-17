namespace NATSInternal.Core.Interfaces.Entities;

internal interface IDebtEntity<T, TUpdateHistory, TData> : ICustomerEngageableEntity<T, TUpdateHistory, TData>
    where T : class, ICustomerEngageableEntity<T, TUpdateHistory, TData>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    long Amount { get; set; }
}