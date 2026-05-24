namespace NATSInternal.Core.Features.Orders;

public class OrderServiceItemDetailResponseDto
{
    #region Constructors
    internal OrderServiceItemDetailResponseDto(OrderServiceItem orderServiceItem)
    {
        Id = orderServiceItem.Id;
        Name = orderServiceItem.Name;
        AmountBeforeVatPerUnit = orderServiceItem.AmountBeforeVatPerUnit;
        VatPercentagePerUnit = orderServiceItem.VatPercentagePerUnit;
        Quantity = orderServiceItem.Quantity;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public long AmountBeforeVatPerUnit { get; }
    public int VatPercentagePerUnit { get; }
    public int Quantity { get; }
    #endregion
}
