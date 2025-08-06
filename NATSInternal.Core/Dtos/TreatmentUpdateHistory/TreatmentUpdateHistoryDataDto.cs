namespace NATSInternal.Core.Dtos;

public class TreatmentUpdateHistoryDataDto
{
    public DateTime StatsDateTime { get; set; }
    public long ServiceAmount { get; set; }
    public decimal ServiceVatAmount { get; set; }
    public string Note { get; set; }
    public TreatmentTherapistUpdateHistoryDataDto Therapist { get; set; }
    public List<TreatmentItemUpdateHistoryDataDto> Items { get; set; }

    public TreatmentUpdateHistoryDataDto() { }
    
    internal TreatmentUpdateHistoryDataDto(Treatment treatment)
    {
        StatsDateTime = treatment.StatsDateTime;
        ServiceAmount = treatment.ServiceAmountBeforeVat;
        ServiceVatAmount = treatment.ServiceVatAmount;
        Note = treatment.Note;
        Therapist = treatment.Therapist != null
            ? new TreatmentTherapistUpdateHistoryDataDto(treatment.Therapist)
            : null;
        Items = treatment.Items
            .Select(i => new TreatmentItemUpdateHistoryDataDto(i))
            .ToList();
    }
}