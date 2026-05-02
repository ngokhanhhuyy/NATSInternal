namespace NATSInternal.Core.Common.Entities;

internal interface IHasProductEntity<TItem> : IHasStatsEntity where TItem : class, IHasProductItemEntity, new()
{
    #region NavigationProperties
    List<TItem> Items { get; }
    #endregion
}