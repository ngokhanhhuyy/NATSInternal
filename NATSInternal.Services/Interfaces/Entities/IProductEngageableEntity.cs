namespace NATSInternal.Services.Interfaces.Entities;

internal interface IProductEngageableEntity<T, TItem, TPhoto, TUpdateHistory>
    :
        IFinancialEngageableEntity<T, TUpdateHistory>,
        IHasPhotoEntity<T, TPhoto>
    where T : class, IUpsertableEntity<T>, new()
    where TItem : class, IProductEngageableItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory :class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    List<TItem> Items { get; set; }
}