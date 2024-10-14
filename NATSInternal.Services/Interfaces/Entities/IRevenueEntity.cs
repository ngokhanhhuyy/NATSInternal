namespace NATSInternal.Services.Interfaces.Entities;

internal interface IRevenueEntity<T, TUpdateHistory>
    : IFinancialEngageableEntity<T, TUpdateHistory>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    long AmountBeforeVat { get; }
    long VatAmount { get; }
    int CustomerId { get; set; }
    Customer Customer { get; set; }
}