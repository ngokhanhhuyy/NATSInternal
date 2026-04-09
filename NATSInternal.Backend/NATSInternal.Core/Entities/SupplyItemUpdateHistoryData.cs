namespace NATSInternal.Core.Entities;

internal class SupplyItemUpdateHistoryData
{
    #region Properties
    public required Guid Id { get; set; }
    public required long AmountPerUnit { get; set; }
    public required int Quantity { get; set; }
    public required string ProductName { get; set; }
    #endregion
}