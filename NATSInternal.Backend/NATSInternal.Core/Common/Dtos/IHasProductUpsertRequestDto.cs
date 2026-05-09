namespace NATSInternal.Core.Common.Dtos;

public interface IHasProductUpsertRequestDto<TItem> : IHasStatsUpsertRequestDto
    where TItem : IHasProductItemUpsertRequestDto
{
    #region Properties
    List<TItem> Items { get; set; }
    #endregion
}