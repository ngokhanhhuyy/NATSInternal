namespace NATSInternal.Core.Dtos;

public class DebtDetailResponseDto
    :
        IHasCustomerBasicResponseDto<DebtExistingAuthorizationResponseDto>,
        IHasStatsDetailResponseDto<DebtUpdateHistoryResponseDto, DebtExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    public long Amount { get; set; }
    public string? Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DebtExistingAuthorizationResponseDto Authorization { get; set; }
    public List<DebtUpdateHistoryResponseDto> UpdateHistories { get; set; }
    #endregion

    #region Constructors
    internal DebtDetailResponseDto(
            Debt debtIncurrence,
            DebtExistingAuthorizationResponseDto authorization)
    {
        Id = debtIncurrence.Id;
        Amount = debtIncurrence.Amount;
        Note = debtIncurrence.Note;
        StatsDateTime = debtIncurrence.StatsDateTime;
        CreatedDateTime = debtIncurrence.CreatedDateTime;
        IsLocked = debtIncurrence.IsLocked();
        Customer = new CustomerBasicResponseDto(debtIncurrence.Customer);
        CreatedUser = new UserBasicResponseDto(debtIncurrence.CreatedUser);
        Authorization = authorization;
        UpdateHistories = debtIncurrence.UpdateHistories
            .Select(updateHistory => new DebtUpdateHistoryResponseDto(updateHistory))
            .ToList();
    }
    #endregion
}