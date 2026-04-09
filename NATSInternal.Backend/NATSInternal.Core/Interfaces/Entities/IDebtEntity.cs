namespace NATSInternal.Core.Interfaces.Entities;

internal interface IDebtEntity<T, TData> : IHasCustomerEntity<T, TData>
    where T : class, IHasCustomerEntity<T, TData>
    where TData : class
{
    long Amount { get; set; }
}