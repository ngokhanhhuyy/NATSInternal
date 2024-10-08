namespace NATSInternal.Services.Dtos;

public class SupplyUpsertRequestDto : IRequestDto
{
    public DateTime? PaidDateTime { get; set; }
    public long ShipmentFee { get; set; }
    public string Note { get; set; }
    public string UpdateReason { get; set; }
    public List<SupplyItemRequestDto> Items { get; set; }
    public List<SupplyPhotoRequestDto> Photos { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        UpdateReason = UpdateReason?.ToNullIfEmpty();
        Items.ForEach(i => i.TransformValues());
        Photos.ForEach(p => p.TransformValues());
    }
}
