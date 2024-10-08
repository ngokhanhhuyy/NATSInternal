namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductExportableEntity<T, TItem, TPhoto, TUpdateHistory>
    :
        IRevenueEntity<T, TUpdateHistory>,
        IProductEngageableEntity<T, TItem, TPhoto, TUpdateHistory>
    where T
        :
            class,
            IRevenueEntity<T, TUpdateHistory>,
            IProductEngageableEntity<T, TItem, TPhoto, TUpdateHistory>,
            new()
    where TItem : class, IProductEngageableItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()

{
    long ProductAmountBeforeVat { get; }
    long ProductVatAmount { get; }
}