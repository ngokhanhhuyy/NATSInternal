namespace NATSInternal.Services.Dtos;

public class ExpenseBasicResponseDto
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public DateTime PaidDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public bool IsLocked { get; set; }
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
        Amount = expense.Amount;
        PaidDateTime = expense.PaidDateTime;
        Category = expense.Category;
        IsLocked = expense.IsLocked;
    }
}