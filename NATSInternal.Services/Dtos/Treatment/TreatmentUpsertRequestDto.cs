namespace NATSInternal.Services.Dtos;

public class TreatmentUpsertRequestDto : IRequestDto
{
    public DateTime? PaidDateTime { get; set; }
    public long ServiceAmount { get; set; }
    public int ServiceVatPercentage { get; set; }
    public string Note { get; set; }
    public int CustomerId { get; set; }
    public int TherapistId { get; set; }
    public string UpdateReason { get; set; }
    public List<TreatmentItemRequestDto> Items { get; set; }
    public List<TreatmentPhotoRequestDto> Photos { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        Items = Items?.Select(i => i.TransformValues()).ToList();
        Photos = Photos?.Select(p => p.TransformValues()).ToList();
    }
}
