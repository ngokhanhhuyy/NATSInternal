namespace NATSInternal.Services.Dtos;

public class ExpenseBasicResponseDto
    :
        IHasStatsBasicResponseDto<ExpenseExistingAuthorizationResponseDto>,
        IHasThumbnailBasicResponseDto
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public DateTime StatsDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsLocked { get; set; }
    public string ThumbnailUrl { get; set; }
    public ExpenseExistingAuthorizationResponseDto Authorization { get; set; }

    internal ExpenseBasicResponseDto(Expense expense)
    {
        MapFromEntity(expense);
    }

    internal ExpenseBasicResponseDto(
            Expense expense,
            ExpenseExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(expense);
        Authorization = authorization;
    }

    private void MapFromEntity(Expense expense)
    {
        Id = expense.Id;
        Amount = expense.Amount;
        StatsDateTime = expense.StatsDateTime;
        Category = expense.Category;
        IsLocked = expense.IsLocked;
        ThumbnailUrl = expense.Photos?.Select(p => p.Url).FirstOrDefault();
    }
}