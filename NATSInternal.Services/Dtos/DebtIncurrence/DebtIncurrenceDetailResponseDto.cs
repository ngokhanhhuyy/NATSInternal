namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceDetailResponseDto
    : IDebtDetailResponseDto<
        DebtIncurrenceUpdateHistoryResponseDto,
        DebtIncurrenceExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DebtIncurrenceExistingAuthorizationResponseDto Authorization { get; set; }
    public List<DebtIncurrenceUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal DebtIncurrenceDetailResponseDto(
            DebtIncurrence debtIncurrence,
            DebtIncurrenceExistingAuthorizationResponseDto authorization)
    {
        Id = debtIncurrence.Id;
        Amount = debtIncurrence.Amount;
        Note = debtIncurrence.Note;
        StatsDateTime = debtIncurrence.StatsDateTime;
        CreatedDateTime = debtIncurrence.CreatedDateTime;
        IsLocked = debtIncurrence.IsLocked;
        Customer = new CustomerBasicResponseDto(debtIncurrence.Customer);
        CreatedUser = new UserBasicResponseDto(debtIncurrence.CreatedUser);
        Authorization = authorization;
        UpdateHistories = debtIncurrence.UpdateHistories?
            .Select(uh => new DebtIncurrenceUpdateHistoryResponseDto(uh))
            .ToList();
    }
}