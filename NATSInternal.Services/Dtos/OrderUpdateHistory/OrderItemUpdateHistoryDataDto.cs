namespace NATSInternal.Services.Dtos;

public class OrderItemUpdateHistoryDataDto
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public decimal VatFactor { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }

    public OrderItemUpdateHistoryDataDto() { }
    
    internal OrderItemUpdateHistoryDataDto(OrderItem item)
    {
        Id = item.Id;
        Amount = item.AmountPerUnit;
        VatFactor = item.VatAmountPerUnit;
        Quantity = item.Quantity;
        ProductName = item.Product.Name;
    }
}