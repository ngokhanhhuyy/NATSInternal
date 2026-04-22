namespace NATSInternal.Domain.Features.Supplies;

public class SupplySnapshot
{
    #region Constructors
    internal SupplySnapshot(Supply supply)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        BillCode = supply.BillCode;
        Items = supply.Items.Select(i => new SupplyItemSnapshot(i)).ToList();
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public long ShipmentFee { get; set; }
    public string? BillCode { get; set; }
    public List<SupplyItemSnapshot> Items { get; }
    #endregion
}