using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Features.Orders;

public class OrderProductItemDetailResponseDto
{
    #region Constructors
    internal OrderProductItemDetailResponseDto(OrderProductItem orderProductItem)
    {
        Id = orderProductItem.Id;
        AmountBeforeVatPerUnit = orderProductItem.AmountBeforeVatPerUnit;
        VatPercentagePerUnit = orderProductItem.VatPercentagePerUnit;
        Quantity = orderProductItem.Quantity;
        Product = new(orderProductItem.Product);
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public long AmountBeforeVatPerUnit { get; }
    public int VatPercentagePerUnit { get; }
    public int Quantity { get; }
    public ProductBasicResponseDto Product { get; }
    #endregion
}
