namespace NATSInternal.Core.Entities;

internal class SupplyUpdateHistoryData
{
    #region Properties
    public required DateTime StatsDateTime { get; set; }
    public required long ShipmentFee { get; set; }
    public required string Note { get; set; }
    public required List<SupplyItemUpdateHistoryDataDto> Items { get; set; }
    #endregion
}