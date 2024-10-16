namespace NATSInternal.Services.Dtos;

public class SupplyItemUpdateHistoryDataDto
{
    public int Id { get; set; }
    public long ProdutAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    
    internal SupplyItemUpdateHistoryDataDto(SupplyItem item)
    {
        Id = item.Id;
        ProdutAmountPerUnit = item.ProductAmountPerUnit;
        Quantity = item.Quantity;
        ProductName = item.Product.Name;
    }
}