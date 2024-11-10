namespace NATSInternal.Services.Dtos;

public class ExpenseDetailResponseDto : IHasStatsDetailResponseDto<
    ExpenseUpdateHistoryResponseDto,
    ExpenseExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountAfterVat { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public ExpenseCategory Category { get; set; }
    public string Note { get; set; }
    public bool IsLocked { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public ExpensePayeeResponseDto Payee { get; set; }
    public List<ExpensePhotoResponseDto> Photos { get; set; }
    public ExpenseExistingAuthorizationResponseDto Authorization { get; set; }
    public List<ExpenseUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal ExpenseDetailResponseDto(
            Expense expense,
            ExpenseExistingAuthorizationResponseDto authorization)
    {
        Id = expense.Id;
        AmountAfterVat = expense.Amount;
        StatsDateTime = expense.StatsDateTime;
        Category = expense.Category;
        Note = expense.Note;
        IsLocked = expense.IsLocked;
        CreatedUser = new UserBasicResponseDto(expense.CreatedUser);
        Payee = new ExpensePayeeResponseDto(expense.Payee);
        Photos = expense.Photos?.Select(p => new ExpensePhotoResponseDto(p)).ToList();
        Authorization = authorization;
        UpdateHistories = expense.UpdateHistories?
            .Select(uh => new ExpenseUpdateHistoryResponseDto(uh))
            .ToList();
    }
}