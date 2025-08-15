namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasProductEntity<T, TItem, TPhoto, TUpdateHistory>
    :
        IHasStatsEntity<T, TUpdateHistory>,
        IHasMultiplePhotosEntity<T, TPhoto>
    where T : class, IUpsertableEntity<T>, new()
    where TItem : class, IHasProductItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory :class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    List<TItem> Items { get; }
}