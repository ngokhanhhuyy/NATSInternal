namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductEngageableUpsertRequestDto<TItem, TPhoto>
    : IFinancialEngageableUpsertRequestDto
    where TItem : IProductEngageableItemRequestDto
    where TPhoto : IPhotoRequestDto
{
    List<TItem> Items { get; set; }
    List<TPhoto> Photos { get; set; }
}