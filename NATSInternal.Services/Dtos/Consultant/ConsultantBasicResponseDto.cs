namespace NATSInternal.Services.Dtos;

public class ConsultantBasicResponseDto
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public DateTime PaidDateTime { get; set; }
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
        Amount = consultant.AmountBeforeVat;
        PaidDateTime = consultant.PaidDateTime;
        IsLocked = consultant.IsLocked;
        Customer = new CustomerBasicResponseDto(consultant.Customer);
    }
}