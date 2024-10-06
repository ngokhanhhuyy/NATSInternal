namespace NATSInternal.Services.Dtos;

public class SupplyItemResponseDto
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public int SuppliedQuantity { get; set; }
    public ProductBasicResponseDto Product { get; set; }

    internal SupplyItemResponseDto(SupplyItem item)
    {
        Id = item.Id;
        Amount = item.AmountPerUnit;
        SuppliedQuantity = item.Quantity;
        Product = new ProductBasicResponseDto(item.Product);
    }
}
