namespace NATSInternal.Services.Dtos;

public class SupplyItemUpdateHistoryDataDto : IHasProductItemUpdateHistoryDataDto
{
    public int Id { get; set; }
    public long ProductAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }

    public SupplyItemUpdateHistoryDataDto() { }
    
    internal SupplyItemUpdateHistoryDataDto(SupplyItem item)
    {
        Id = item.Id;
        ProductAmountPerUnit = item.ProductAmountPerUnit;
        Quantity = item.Quantity;
        ProductName = item.Product.Name;
    }
}