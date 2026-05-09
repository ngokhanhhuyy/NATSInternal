namespace NATSInternal.Core.Features.Orders;

public class OrderDetailServiceItemResponseDto
{
    #region Constructors
    internal OrderDetailServiceItemResponseDto(OrderServiceItem orderServiceItem)
    {
        Id = orderServiceItem.Id;
        AmountBeforeVatPerUnit = orderServiceItem.AmountBeforeVatPerUnit;
        VatAmountPerUnit = orderServiceItem.VatAmountPerUnit;
        Quantity = orderServiceItem.Quantity;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public long AmountBeforeVatPerUnit { get; }
    public long VatAmountPerUnit { get; }
    public int Quantity { get; }
    #endregion
}