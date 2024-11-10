namespace NATSInternal.Services.Dtos;

public class TreatmentBasicResponseDto : IExportProductBasicResponseDto<TreatmentExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public long Amount { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public TreatmentExistingAuthorizationResponseDto Authorization { get; set; }

    internal TreatmentBasicResponseDto(Treatment treatment)
    {
        MapFromEntity(treatment);
    }

    internal TreatmentBasicResponseDto(
            Treatment treatment,
            TreatmentExistingAuthorizationResponseDto authorizationResponseDto)
    {
        MapFromEntity(treatment);
        Authorization = authorizationResponseDto;
    }

    private void MapFromEntity(Treatment treatment)
    {
        Id = treatment.Id;
        StatsDateTime = treatment.StatsDateTime;
        Amount = treatment.AmountAfterVat;
        IsLocked = treatment.IsLocked;
        Customer = new CustomerBasicResponseDto(treatment.Customer);
    }
}