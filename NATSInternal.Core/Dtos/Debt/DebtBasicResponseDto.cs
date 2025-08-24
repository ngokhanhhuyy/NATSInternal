namespace NATSInternal.Core.Dtos;

public class DebtBasicResponseDto : IHasCustomerBasicResponseDto<DebtExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    public DebtType Type { get; set; }
    public long Amount { get; set; }
    public string? Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public DebtExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal DebtBasicResponseDto(Debt debt)
    {
        Id = debt.Id;
        Amount = debt.Amount;
        Note = debt.Note;
        StatsDateTime = debt.StatsDateTime;
        IsLocked = debt.IsLocked();
        Customer = new CustomerBasicResponseDto(debt.Customer);
    }

    internal DebtBasicResponseDto(Debt debt, DebtExistingAuthorizationResponseDto authorization) : this(debt)
    {
        Authorization = authorization;
    }
    #endregion
}
