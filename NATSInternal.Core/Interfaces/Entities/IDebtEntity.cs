namespace NATSInternal.Core.Interfaces.Entities;

internal interface IDebtEntity<T, TData> : ICustomerEngageableEntity<T, TData>
    where T : class, ICustomerEngageableEntity<T, TData>
    where TData : class
{
    long Amount { get; set; }
}