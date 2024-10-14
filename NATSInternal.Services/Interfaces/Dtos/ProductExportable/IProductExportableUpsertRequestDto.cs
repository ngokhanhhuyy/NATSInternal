namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductExportableUpsertRequestDto<TItem, TPhoto>
    :
        IProductEngageableUpsertRequestDto<TItem, TPhoto>,
        ICustomerEngageableUpsertRequestDto
    where TItem : IProductEngageableItemRequestDto
    where TPhoto : IPhotoRequestDto
{
}
