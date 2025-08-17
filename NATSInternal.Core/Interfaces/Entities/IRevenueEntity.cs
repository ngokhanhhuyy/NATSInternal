namespace NATSInternal.Core.Interfaces.Entities;

internal interface IRevenueEntity<T, TUpdateHistory, TData> : IHasStatsEntity<T, TUpdateHistory, TData>
    where T : class, IHasStatsEntity<T, TUpdateHistory, TData>
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TData>
    where TData : class
{
    #region Properties
    long AmountBeforeVat { get; }
    long VatAmount { get; }
    Guid CustomerId { get; set; }
    Customer Customer { get; set; }
    #endregion
}