namespace NATSInternal.Core.Entities;

internal class DebtUpdateHistoryData
{
    #region Constructors
    private DebtUpdateHistoryData() { }
    public DebtUpdateHistoryData(Debt debt)
    {
        Amount = debt.Amount;
        Note = debt.Note;
        StatsDateTime = debt.StatsDateTime;
    }
    #endregion

    #region Properties
    public long Amount { get; set; }
    public string? Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    #endregion
}