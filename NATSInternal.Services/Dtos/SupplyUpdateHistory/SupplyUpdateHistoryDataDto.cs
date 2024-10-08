namespace NATSInternal.Services.Dtos;

public class SupplyUpdateHistoryDataDto
{
    public DateTime PaidDateTime { get; set; }
    public long ShipmentFee { get; set; }
    public string Note { get; set; }
    public List<SupplyItemUpdateHistoryDataDto> Items { get; set; }
    
    internal SupplyUpdateHistoryDataDto(Supply supply)
    {
        PaidDateTime = supply.SuppliedDateTime;
        ShipmentFee = supply.ShipmentFee;
        Note = supply.Note;
        Items = supply.Items?
            .Select(si => new SupplyItemUpdateHistoryDataDto(si))
            .ToList();
    }
}