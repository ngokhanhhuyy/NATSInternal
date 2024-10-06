namespace NATSInternal.Services.Dtos;

public class TreatmentBasicResponseDto
{
    public int Id { get; set; }
    public DateTime PaidDateTime { get; set; }
    public long Amount { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public TreatmentAuthorizationResponseDto Authorization { get; set; }

    internal TreatmentBasicResponseDto(Treatment treatment)
    {
        MapFromEntity(treatment);
    }

    internal TreatmentBasicResponseDto(
            Treatment treatment,
            TreatmentAuthorizationResponseDto authorizationResponseDto)
    {
        MapFromEntity(treatment);
        Authorization = authorizationResponseDto;
    }

    private void MapFromEntity(Treatment treatment)
    {
        Id = treatment.Id;
        PaidDateTime = treatment.PaidDateTime;
        Amount = treatment.AmountBeforeVat;
        IsLocked = treatment.IsLocked;
        Customer = new CustomerBasicResponseDto(treatment.Customer);
    }
}