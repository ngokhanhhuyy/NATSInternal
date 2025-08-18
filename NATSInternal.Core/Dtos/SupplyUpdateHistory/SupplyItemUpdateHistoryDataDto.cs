namespace NATSInternal.Core.Dtos;

public class SupplyItemUpdateHistoryDataDto : IHasProductItemUpdateHistoryDataDto
{
    public int Id { get; set; }
    public long AmountBeforeVatPerUnit { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }

    public SupplyItemUpdateHistoryDataDto() { }
    
    internal SupplyItemUpdateHistoryDataDto(SupplyItem item)
    {
        Id = item.Id;
        AmountBeforeVatPerUnit = item.AmountPerUnit;
        Quantity = item.Quantity;
        ProductName = item.Product.Name;
    }
}