namespace NATSInternal.Services.Dtos;

public class ExpenseBasicResponseDto
    :
        IFinancialEngageableBasicResponseDto<ExpenseAuthorizationResponseDto>,
        IHasThumbnailBasicResponseDto
{
    public int Id { get; set; }
    public long AmountAfterVat { get; set; }
    public DateTime StatsDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsLocked { get; set; }
    public string ThumbnailUrl { get; set; }
    public ExpenseAuthorizationResponseDto Authorization { get; set; }

    internal ExpenseBasicResponseDto(Expense expense)
    {
        MapFromEntity(expense);
    }

    internal ExpenseBasicResponseDto(
            Expense expense,
            ExpenseAuthorizationResponseDto authorization)
    {
        MapFromEntity(expense);
        Authorization = authorization;
    }

    private void MapFromEntity(Expense expense)
    {
        Id = expense.Id;
        AmountAfterVat = expense.Amount;
        StatsDateTime = expense.StatsDateTime;
        Category = expense.Category;
        IsLocked = expense.IsLocked;
        ThumbnailUrl = expense.Photos?.Select(p => p.Url).FirstOrDefault();
    }
}