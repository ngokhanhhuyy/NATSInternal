namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductExportableUpsertRequestDto<TItem, TPhoto>
    :
        IHasProductUpsertRequestDto<TItem, TPhoto>,
        IHasCustomerUpsertRequestDto
    where TItem : IHasProductItemRequestDto
    where TPhoto : IPhotoRequestDto;