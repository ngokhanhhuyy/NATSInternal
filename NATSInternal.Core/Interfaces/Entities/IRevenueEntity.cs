namespace NATSInternal.Core.Interfaces.Entities;

internal interface IRevenueEntity<T, TUpdateHistory> : IHasStatsEntity<T, TUpdateHistory>
    where T : class, IHasStatsEntity<T, TUpdateHistory>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>
{
    long AmountBeforeVat { get; }
    long VatAmount { get; }
    Guid CustomerId { get; set; }
    Customer Customer { get; set; }
}