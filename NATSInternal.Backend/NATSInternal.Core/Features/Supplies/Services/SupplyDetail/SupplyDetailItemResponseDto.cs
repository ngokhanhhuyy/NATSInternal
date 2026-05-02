using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyDetailItemResponseDto
{
    #region Constructors
    internal SupplyDetailItemResponseDto(SupplyItem supplyItem)
    {
        Id = supplyItem.Id;
        AmountPerUnit = supplyItem.AmountPerUnit;
        Quantity = supplyItem.Quantity;
        Product = new(supplyItem.Product);
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public long AmountPerUnit { get; }
    public int Quantity { get; }
    public ProductBasicResponseDto Product { get; }
    #endregion
}