namespace NATSInternal.Services.Dtos;

public class OrderItemResponseDto : IProductEngageableItemResponseDto
{
    public int Id { get; set; }
    public long ProductAmountPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public ProductBasicResponseDto Product { get; set; }

    internal OrderItemResponseDto(OrderItem item)
    {
        Id = item.Id;
        ProductAmountPerUnit = item.ProductAmountPerUnit;
        VatAmountPerUnit = item.VatAmountPerUnit;
        Quantity = item.Quantity;
        Product = new ProductBasicResponseDto(item.Product);
    }
}