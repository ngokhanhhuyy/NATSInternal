using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Debts;

public class DebtBasicResponseDto
{
    #region Constructors
    internal DebtBasicResponseDto(Debt debt)
    {
        Id = debt.Id;
        Amount = debt.Amount;
        StatsDate = debt.StatsDate;
        CustomerId = debt.CustomerId;
    }

    internal DebtBasicResponseDto(Debt debt, DebtExistingAuthorizationResponseDto authorization) : this(debt)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public long Amount { get; }
    public DateOnly StatsDate { get; }
    public int CustomerId { get; }
    public DebtExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}