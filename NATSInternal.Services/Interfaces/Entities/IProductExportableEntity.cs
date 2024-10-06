namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductExportableEntity<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>
    :
        IRevenueEntity<T, TUser, TUpdateHistory>,
        IProductEngageableEntity<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>
    where T
        :
            class,
            IRevenueEntity<T, TUser, TUpdateHistory>,
            IProductEngageableEntity<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>,
            new()
    where TItem : class, IProductEngageableItemEntity<TItem, TProduct>, new()
    where TProduct : class, IProductEntity<TProduct>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()

{
    long ProductAmountBeforeVat { get; }
    long ProductVatAmount { get; }
}