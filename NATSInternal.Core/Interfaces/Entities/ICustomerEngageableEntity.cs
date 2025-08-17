namespace NATSInternal.Core.Interfaces.Entities;

internal interface ICustomerEngageableEntity<T, TUpdateHistory, TData> : IHasStatsEntity<T, TUpdateHistory, TData>
    where T : class, IUpsertableEntity<T>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    Guid CustomerId { get; set; }
    Customer Customer { get; set; }
}