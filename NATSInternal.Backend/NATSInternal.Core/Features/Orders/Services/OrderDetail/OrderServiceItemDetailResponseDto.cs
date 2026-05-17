namespace NATSInternal.Core.Features.Orders;

public class OrderServiceItemDetailResponseDto
{
    #region Constructors
    internal OrderServiceItemDetailResponseDto(OrderServiceItem orderServiceItem)
    {
        Id = orderServiceItem.Id;
        Name = orderServiceItem.Name;
        AmountBeforeVatPerUnit = orderServiceItem.AmountBeforeVatPerUnit;
        VatAmountPerUnit = orderServiceItem.VatAmountPerUnit;
        Quantity = orderServiceItem.Quantity;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public long AmountBeforeVatPerUnit { get; }
    public long VatAmountPerUnit { get; }
    public int Quantity { get; }
    #endregion
}
