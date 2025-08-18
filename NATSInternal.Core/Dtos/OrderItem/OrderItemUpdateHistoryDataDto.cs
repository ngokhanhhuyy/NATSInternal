namespace NATSInternal.Core.Dtos;

public class OrderItemUpdateHistoryDataDto : IProductExportableItemUpdateHistoryDataDto
{
    #region Constructors
    public OrderItemUpdateHistoryDataDto() { }

    internal OrderItemUpdateHistoryDataDto(OrderItemUpdateHistoryData item)
    {
        Id = item.Id;
        AmountBeforeVatPerUnit = item.AmountBeforeVatPerUnit;
        VatAmountPerUnit = item.VatAmountPerUnit;
        Quantity = item.Quantity;
        ProductName = item.ProductName;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; set; }
    public long AmountBeforeVatPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = string.Empty;
    #endregion
}