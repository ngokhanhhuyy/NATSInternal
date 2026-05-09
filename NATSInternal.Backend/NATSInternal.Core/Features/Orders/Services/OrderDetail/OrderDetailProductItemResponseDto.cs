using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Features.Orders;

public class OrderDetailProductItemResponseDto
{
    #region Constructors
    internal OrderDetailProductItemResponseDto(OrderProductItem orderProductItem)
    {
        Id = orderProductItem.Id;
        AmountBeforeVatPerUnit = orderProductItem.AmountBeforeVatPerUnit;
        VatAmountPerUnit = orderProductItem.VatAmountPerUnit;
        Quantity = orderProductItem.Quantity;
        Product = new(orderProductItem.Product);
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public long AmountBeforeVatPerUnit { get; }
    public long VatAmountPerUnit { get; }
    public int Quantity { get; }
    public ProductBasicResponseDto Product { get; }
    #endregion
}