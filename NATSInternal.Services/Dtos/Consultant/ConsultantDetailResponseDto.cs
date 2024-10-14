namespace NATSInternal.Services.Dtos;

public class ConsultantDetailResponseDto : IFinancialEngageableDetailResponseDto<
    ConsultantUpdateHistoryResponseDto,
    ConsultantAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? LastUpdatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public ConsultantAuthorizationResponseDto Authorization { get; set; }
    public List<ConsultantUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal ConsultantDetailResponseDto(
            Consultant consultant,
            ConsultantAuthorizationResponseDto authorization)
    {
        Id = consultant.Id;
        Amount = consultant.AmountBeforeVat;
        Note = consultant.Note;
        StatsDateTime = consultant.StatsDateTime;
        CreatedDateTime = consultant.CreatedDateTime;
        LastUpdatedDateTime = consultant.LastUpdatedDateTime;
        IsLocked = consultant.IsLocked;
        Customer = new CustomerBasicResponseDto(consultant.Customer);
        CreatedUser = new UserBasicResponseDto(consultant.CreatedUser);
        Authorization = authorization;
        UpdateHistories = consultant.UpdateHistories
            .Select(uh => new ConsultantUpdateHistoryResponseDto(uh))
            .ToList();
    }
}