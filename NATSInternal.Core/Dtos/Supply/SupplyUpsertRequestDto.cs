namespace NATSInternal.Core.Dtos;

public class SupplyUpsertRequestDto
    : IHasProductUpsertRequestDto<SupplyItemRequestDto, SupplyPhotoRequestDto>
{
    public DateTime? StatsDateTime { get; set; }
    public long ShipmentFee { get; set; }
    public string Note { get; set; }
    public List<SupplyItemRequestDto> Items { get; set; }
    public List<SupplyPhotoRequestDto> Photos { get; set; }
    public string UpdatedReason { get; set; }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        UpdatedReason = UpdatedReason?.ToNullIfEmpty();
        Items.ForEach(i => i.TransformValues());
        Photos.ForEach(p => p.TransformValues());
    }
}
