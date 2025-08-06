namespace NATSInternal.Core.Interfaces.Entities;

internal interface ICustomerEngageableEntity<T, TUpdateHistory>
    : IHasStatsEntity<T, TUpdateHistory>
    where T : class, IUpsertableEntity<T>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    int CustomerId { get; set; }
    Customer Customer { get; set; }
}