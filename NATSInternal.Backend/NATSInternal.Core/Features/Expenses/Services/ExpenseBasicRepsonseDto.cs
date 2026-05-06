using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Expenses;

public class ExpenseBasicResponseDto
{
    #region Constructors
    internal ExpenseBasicResponseDto(Expense expense)
    {
        Id = expense.Id;
        Amount = expense.Amount;
        Type = expense.Type;
        StatsDate = expense.StatsDate;
    }

    internal ExpenseBasicResponseDto(
        Expense expense,
        ExpenseExistingAuthorizationResponseDto authorization) : this(expense)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public long Amount { get; }
    public ExpenseType Type { get; }
    public DateOnly StatsDate { get; }
    public PhotoBasicResponseDto? Thumbnail { get; }
    public ExpenseExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}