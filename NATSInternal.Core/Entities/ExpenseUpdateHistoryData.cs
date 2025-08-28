namespace NATSInternal.Core.Entities;

internal record ExpenseUpdateHistoryData
{
    #region Properties
    public required long Amount { get; set; }
    public required DateTime StatsDateTime { get; set; }
    public required ExpenseCategory Category { get; set; }
    public required string Note { get; set; }
    public required string PayeeName { get; set; }
    #endregion
}