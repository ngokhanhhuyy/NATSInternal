namespace NATSInternal.Services.Dtos;

public class TreatmentBasicResponseDto : IProductExportableBasicResponseDto<TreatmentExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public long AmountAfterVat { get; set; }
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
        AmountAfterVat = treatment.AmountAfterVat;
        IsLocked = treatment.IsLocked;
        Customer = new CustomerBasicResponseDto(treatment.Customer);
    }
}