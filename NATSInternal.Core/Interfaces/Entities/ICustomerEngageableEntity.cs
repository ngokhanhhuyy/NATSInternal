namespace NATSInternal.Core.Interfaces.Entities;

internal interface ICustomerEngageableEntity<T, TData> : IHasStatsEntity<T, TData>
    where T : class, IUpsertableEntity<T>
    where TData : class
{
    Guid CustomerId { get; set; }
    Customer Customer { get; set; }
}