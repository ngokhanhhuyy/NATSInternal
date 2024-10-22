namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceDetailResponseDto
    : IDebtDetailResponseDto<
        DebtIncurrenceUpdateHistoryResponseDto,
        DebtIncurrenceAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountAfterVat { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DebtIncurrenceAuthorizationResponseDto Authorization { get; set; }
    public List<DebtIncurrenceUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal DebtIncurrenceDetailResponseDto(
            DebtIncurrence debtIncurrence,
            DebtIncurrenceAuthorizationResponseDto authorization)
    {
        Id = debtIncurrence.Id;
        AmountAfterVat = debtIncurrence.Amount;
        Note = debtIncurrence.Note;
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