namespace NATSInternal.Services.Dtos;

public class TreatmentUpdateHistoryDataDto
{
    public DateTime PaidDateTime { get; set; }
    public long ServiceAmount { get; set; }
    public decimal ServiceVatFactor { get; set; }
    public string Note { get; set; }
    public TreatmentTherapistUpdateHistoryDataDto Therapist { get; set; }
    public List<TreatmentItemUpdateHistoryDataDto> Items { get; set; }
    
    internal TreatmentUpdateHistoryDataDto(Treatment treatment)
    {
        PaidDateTime = treatment.PaidDateTime;
        ServiceAmount = treatment.ServiceAmountBeforeVat;
        ServiceVatFactor = treatment.ServiceVatAmount;
        Note = treatment.Note;
        Therapist = new TreatmentTherapistUpdateHistoryDataDto(treatment.Therapist);
        Items = treatment.Items
            .Select(i => new TreatmentItemUpdateHistoryDataDto(i))
            .ToList();
    }
}