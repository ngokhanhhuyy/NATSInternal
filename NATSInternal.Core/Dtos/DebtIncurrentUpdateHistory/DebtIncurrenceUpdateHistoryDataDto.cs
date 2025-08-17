namespace NATSInternal.Core.Dtos;

public class DebtIncurrenceUpdateHistoryDataDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }

    public DebtIncurrenceUpdateHistoryDataDto() { }
    
    internal DebtIncurrenceUpdateHistoryDataDto(Debt debtIncurrence)
    {
        Amount = debtIncurrence.Amount;
        Note = debtIncurrence.Note;
        StatsDateTime = debtIncurrence.StatsDateTime;
    }
}