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
    public DebtExistingAuthorizationResponseDto? Authorization { get; set; }
    public List<DebtUpdateHistoryResponseDto> UpdateHistories { get; set; }
    #endregion

    #region Constructors
    internal DebtDetailResponseDto(Debt debt)
    {
        Id = debt.Id;
        Amount = debt.Amount;
        Note = debt.Note;
        StatsDateTime = debt.StatsDateTime;
        CreatedDateTime = debt.CreatedDateTime;
        IsLocked = debt.IsLocked();
        Customer = new CustomerBasicResponseDto(debt.Customer);
        CreatedUser = new UserBasicResponseDto(debt.CreatedUser);
        UpdateHistories = debt.UpdateHistories
            .Select(updateHistory => new DebtUpdateHistoryResponseDto(updateHistory))
            .ToList();
    }
    
    internal DebtDetailResponseDto(Debt debt, DebtExistingAuthorizationResponseDto authorizationResponseDto) : this(debt)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion
}