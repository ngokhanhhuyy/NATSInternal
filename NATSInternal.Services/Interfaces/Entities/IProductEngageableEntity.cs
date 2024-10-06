namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductEngageableEntity<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>
    :
        IFinancialEngageableEntity<T, TUser, TUpdateHistory>,
        IHasPhotoEntity<T, TPhoto>
    where T : class, IUpsertableEntity<T>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TItem : class, IProductEngageableItemEntity<TItem, TProduct>, new()
    where TProduct : class, IProductEntity<TProduct>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory :class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    List<TItem> Items { get; set; }
}