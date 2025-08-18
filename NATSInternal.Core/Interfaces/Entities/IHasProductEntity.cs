namespace NATSInternal.Core.Interfaces.Entities;

internal interface IHasProductEntity<T, TItem, TData> : IHasStatsEntity<T, TData>, IHasPhotosEntity<T>
    where T : class, IUpsertableEntity<T>, new()
    where TItem : class, IHasProductItemEntity<TItem>, new()
    where TData : class
{
    #region Properties
    List<TItem> Items { get; }
    #endregion
}