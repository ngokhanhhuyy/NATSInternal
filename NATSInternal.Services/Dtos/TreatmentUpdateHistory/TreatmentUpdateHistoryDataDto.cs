namespace NATSInternal.Services.Dtos;

public class TreatmentUpdateHistoryDataDto
{
    public DateTime StatsDateTime { get; set; }
    public long ServiceAmount { get; set; }
    public decimal ServiceVatAmount { get; set; }
    public string Note { get; set; }
    public TreatmentTherapistUpdateHistoryDataDto Therapist { get; set; }
    public List<TreatmentItemUpdateHistoryDataDto> Items { get; set; }
    
    internal TreatmentUpdateHistoryDataDto(Treatment treatment)
    {
        StatsDateTime = treatment.StatsDateTime;
        ServiceAmount = treatment.ServiceAmountBeforeVat;
        ServiceVatAmount = treatment.ServiceVatAmount;
        Note = treatment.Note;
        Therapist = new TreatmentTherapistUpdateHistoryDataDto(treatment.Therapist);
        Items = treatment.Items
            .Select(i => new TreatmentItemUpdateHistoryDataDto(i))
            .ToList();
    }
}