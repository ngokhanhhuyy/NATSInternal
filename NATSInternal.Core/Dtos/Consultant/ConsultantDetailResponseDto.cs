namespace NATSInternal.Core.Dtos;

public class ConsultantDetailResponseDto : IHasStatsDetailResponseDto<
    ConsultantUpdateHistoryResponseDto,
    ConsultantExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountBeforeVat { get; set; }
    public long VatAmount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public ConsultantExistingAuthorizationResponseDto Authorization { get; set; }
    public List<ConsultantUpdateHistoryResponseDto> UpdateHistories { get; set; }

    internal ConsultantDetailResponseDto(
            Consultant consultant,
            ConsultantExistingAuthorizationResponseDto authorization)
    {
        Id = consultant.Id;
        AmountBeforeVat = consultant.AmountBeforeVat;
        Note = consultant.Note;
        StatsDateTime = consultant.StatsDateTime;
        CreatedDateTime = consultant.CreatedDateTime;
        IsLocked = consultant.IsLocked;
        Customer = new CustomerBasicResponseDto(consultant.Customer);
        CreatedUser = new UserBasicResponseDto(consultant.CreatedUser);
        Authorization = authorization;
        UpdateHistories = consultant.UpdateHistories?
            .Select(uh => new ConsultantUpdateHistoryResponseDto(uh))
            .ToList();
    }
}