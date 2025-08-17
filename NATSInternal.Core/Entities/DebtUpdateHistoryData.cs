namespace NATSInternal.Core.Entities;

internal class DebtUpdateHistoryData
{
    #region Properties
    public required long Amount { get; set; }
    public required string? Note { get; set; }
    public required DateTime StatsDateTime { get; set; }
    #endregion
}