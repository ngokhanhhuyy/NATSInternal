namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductExportableItemRequestDto : IProductEngageableItemRequestDto
{
    long VatAmountPerUnit { get; }
}
