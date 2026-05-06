using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Expenses;

public class ExpenseDetailResponseDto
{
    #region Constructors
    internal ExpenseDetailResponseDto(Expense expense, ExpenseExistingAuthorizationResponseDto authorization)
    {
        Id = expense.Id;
        StatsDate = expense.StatsDate;
        Amount = expense.Amount;
        Note = expense.Note;
        CreatedDateTime = expense.CreatedDateTime;
        LastUpdatedDateTime = expense.LastUpdatedDateTime;
        DeletedDateTime = expense.DeletedDateTime;
        CreatedUser = new(expense.CreatedUser);
        Authorization = authorization;

        if (expense.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(expense.LastUpdatedUser);
        }

        if (expense.DeletedUser is not null)
        {
            DeletedUser = new(expense.DeletedUser);
        }
    }
    #endregion

    #region Properties
    public int Id { get; }
    public DateOnly StatsDate { get; }
    public long Amount { get; }
    public string? Note { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? DeletedDateTime { get; }
    public UserBasicResponseDto? DeletedUser { get; }
    public ExpenseExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}