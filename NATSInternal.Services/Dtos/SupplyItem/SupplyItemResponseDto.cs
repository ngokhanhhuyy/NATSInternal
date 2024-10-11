namespace NATSInternal.Services.Dtos;

public class SupplyItemResponseDto : IProductEngageableItemResponseDto
{
    public int Id { get; set; }
    public long AmountBeforeVatPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public ProductBasicResponseDto Product { get; set; }

    internal SupplyItemResponseDto(SupplyItem item)
    {
        Id = item.Id;
        AmountBeforeVatPerUnit = item.AmountPerUnit;
        Quantity = item.Quantity;
        Product = new ProductBasicResponseDto(item.Product);
    }
}
