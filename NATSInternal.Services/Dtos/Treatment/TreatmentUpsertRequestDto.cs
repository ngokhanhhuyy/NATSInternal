namespace NATSInternal.Services.Dtos;

public class TreatmentUpsertRequestDto
    : IProductExportableUpsertRequestDto<TreatmentItemRequestDto, TreatmentPhotoRequestDto>
{
    public DateTime? StatsDateTime { get; set; }
    public long ServiceAmountBeforeVat { get; set; }
    public long ServiceVatAmount { get; set; }
    public string Note { get; set; }
    public int CustomerId { get; set; }
    public int? TherapistId { get; set; }
    public List<TreatmentItemRequestDto> Items { get; set; }
    public List<TreatmentPhotoRequestDto> Photos { get; set; }
    public string UpdatedReason { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        Items?.ForEach(i => i.TransformValues());
        Photos?.ForEach(p => p.TransformValues());
    }
}
