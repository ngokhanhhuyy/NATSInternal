namespace NATSInternal.Services.Dtos;

public class ConsultantBasicResponseDto
    : IFinancialEngageableBasicResponseDto<ConsultantExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountAfterVat { get; set; }
    public DateTime StatsDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public ConsultantExistingAuthorizationResponseDto Authorization { get; set; }

    internal ConsultantBasicResponseDto(Consultant consultant)
    {
        MapFromEntity(consultant);
    }

    internal ConsultantBasicResponseDto(
            Consultant consultant,
            ConsultantExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(consultant);
        Authorization = authorization;
    }

    private void MapFromEntity(Consultant consultant)
    {
        Id = consultant.Id;
        AmountAfterVat = consultant.AmountBeforeVat;
        StatsDateTime = consultant.StatsDateTime;
        IsLocked = consultant.IsLocked;
        Customer = new CustomerBasicResponseDto(consultant.Customer);
    }
}