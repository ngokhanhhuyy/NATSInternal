namespace NATSInternal.Core.Dtos;

public class ExpenseUpdateHistoryDataDto : IDebtUpdateHistoryDataDto
{
    public long Amount { get; set; }
    public DateTime StatsDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public string Note { get; set; }
    public string PayeeName { get; set; }

    public ExpenseUpdateHistoryDataDto() { }
    
    internal ExpenseUpdateHistoryDataDto(Expense expense)
    {
        Amount = expense.Amount;
        StatsDateTime = expense.StatsDateTime;
        Category = expense.Category;
        Note = expense.Note;
        PayeeName = expense.Payee.Name;
    }
}