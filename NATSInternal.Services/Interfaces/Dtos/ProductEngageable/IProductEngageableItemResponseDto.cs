namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableItemResponseDto
{
    int Id { get; set; }
    long AmountBeforeVatPerUnit { get; }
    decimal VatAmountPerUnit { get; }
    int Quantity { get; }
    ProductBasicResponseDto Product { get; }
}