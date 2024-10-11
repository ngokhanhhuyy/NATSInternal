namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableItemResponseDto
{
    int Id { get; set; }
    long AmountBeforeVatPerUnit { get; }
    long VatAmountPerUnit { get; }
    int Quantity { get; }
    ProductBasicResponseDto Product { get; }
}