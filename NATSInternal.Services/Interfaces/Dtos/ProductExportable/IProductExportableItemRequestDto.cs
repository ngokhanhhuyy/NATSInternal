namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductExportableItemRequestDto : IHasProductItemRequestDto
{
    long VatAmountPerUnit { get; }
}
