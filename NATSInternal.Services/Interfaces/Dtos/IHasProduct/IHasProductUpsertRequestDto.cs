namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasProductUpsertRequestDto<TItem, TPhoto> : IHasStatsUpsertRequestDto
    where TItem : IHasProductItemRequestDto
    where TPhoto : IPhotoRequestDto
{
    List<TItem> Items { get; set; }
    List<TPhoto> Photos { get; set; }
}