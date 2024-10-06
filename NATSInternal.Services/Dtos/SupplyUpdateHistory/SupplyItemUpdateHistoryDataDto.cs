namespace NATSInternal.Services.Dtos;

public class SupplyItemUpdateHistoryDataDto
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public int SuppliedQuantity { get; set; }
    public string ProductName { get; set; }
    
    internal SupplyItemUpdateHistoryDataDto(SupplyItem item)
    {
        Id = item.Id;
        Amount = item.AmountPerUnit;
        SuppliedQuantity = item.Quantity;
        ProductName = item.Product.Name;
    }
}