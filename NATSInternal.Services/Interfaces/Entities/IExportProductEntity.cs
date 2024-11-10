namespace NATSInternal.Services.Interfaces.Entities;

internal interface IExportProductEntity<T, TItem, TPhoto, TUpdateHistory>
    :
        IRevenueEntity<T, TUpdateHistory>,
        IHasProductEntity<T, TItem, TPhoto, TUpdateHistory>
    where T :
        class,
        IRevenueEntity<T, TUpdateHistory>,
        IHasProductEntity<T, TItem, TPhoto, TUpdateHistory>,
        new()
    where TItem : class, IHasProductItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()

{
    long ProductAmountBeforeVat { get; }
    long ProductVatAmount { get; }
    static abstract Expression<Func<T, long>> AmountAfterVatExpression { get; }
}