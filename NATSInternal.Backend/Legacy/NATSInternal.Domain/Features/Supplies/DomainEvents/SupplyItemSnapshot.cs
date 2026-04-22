namespace NATSInternal.Domain.Features.Supplies;

public class SupplyItemSnapshot
{
    #region Constructors
    internal SupplyItemSnapshot(SupplyItem supplyItem)
    {
        Id = supplyItem.Id;
        AmountPerUnit = supplyItem.AmountPerUnit;
        Quantity = supplyItem.Quantity;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public long AmountPerUnit { get; set; }
    public int Quantity { get; set; }
    #endregion
}