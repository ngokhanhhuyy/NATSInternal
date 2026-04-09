namespace NATSInternal.Core.Interfaces.Dtos;

public interface IProductExportableItemRequestDto : IHasProductItemRequestDto
{
    long VatAmountPerUnit { get; }
}
