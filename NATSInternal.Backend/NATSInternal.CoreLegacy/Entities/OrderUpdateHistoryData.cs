namespace NATSInternal.Core.Entities;

internal class OrderUpdateHistoryData
{
    #region Properties
    public DateTime StatsDateTime { get; set; }
    public string? Note { get; set; }
    public int CustomerId { get; set; }
    public List<OrderItemUpdateHistoryDataDto> Items { get; set; } = new();
    #endregion
}
