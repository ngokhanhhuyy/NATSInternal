namespace NATSInternal.Services.Dtos;

public class SupplyUpdateHistoryDataDto
{
    public DateTime StatsDateTime { get; set; }
    public long ShipmentFee { get; set; }
    public string Note { get; set; }
    public List<SupplyItemUpdateHistoryDataDto> Items { get; set; }

    public SupplyUpdateHistoryDataDto() { }
    
    internal SupplyUpdateHistoryDataDto(Supply supply)
    {
        StatsDateTime = supply.StatsDateTime;
        ShipmentFee = supply.ShipmentFee;
        Note = supply.Note;
        Items = supply.Items?
            .Select(si => new SupplyItemUpdateHistoryDataDto(si))
            .ToList();
    }
}