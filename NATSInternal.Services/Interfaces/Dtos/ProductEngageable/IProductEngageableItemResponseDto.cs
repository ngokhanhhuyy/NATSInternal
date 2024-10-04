namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductEngageableItemResponseDto
{
    int Id { get; set; }
    long AmountBeforeVatPerUnit { get; set; }
    decimal VatAmount { get; set; }
    int Quantity { get; set; }
    ProductBasicResponseDto Product { get; set; }
}