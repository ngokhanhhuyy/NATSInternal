namespace NATSInternal.Services.Dtos;

public class ConsultantBasicResponseDto
    : IFinancialEngageableBasicResponseDto<ConsultantAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountBeforeVat { get; set; }
    public DateTime StatsDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public ConsultantAuthorizationResponseDto Authorization { get; set; }

    internal ConsultantBasicResponseDto(Consultant consultant)
    {
        MapFromEntity(consultant);
    }

    internal ConsultantBasicResponseDto(
            Consultant consultant,
            ConsultantAuthorizationResponseDto authorization)
    {
        MapFromEntity(consultant);
        Authorization = authorization;
    }

    private void MapFromEntity(Consultant consultant)
    {
        Id = consultant.Id;
        AmountBeforeVat = consultant.AmountBeforeVat;
        StatsDateTime = consultant.StatsDateTime;
        IsLocked = consultant.IsLocked;
        Customer = new CustomerBasicResponseDto(consultant.Customer);
    }
}