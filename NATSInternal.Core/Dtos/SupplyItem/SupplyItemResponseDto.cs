namespace NATSInternal.Core.Dtos;

public class SupplyItemResponseDto : IHasProductItemResponseDto
{
    public int Id { get; set; }
    public long ProductAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public ProductBasicResponseDto Product { get; set; }

    internal SupplyItemResponseDto(SupplyItem item)
    {
        Id = item.Id;
        ProductAmountPerUnit = item.AmountPerUnit;
        Quantity = item.Quantity;
        Product = new ProductBasicResponseDto(item.Product);
    }
}
