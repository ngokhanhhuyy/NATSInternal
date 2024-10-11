namespace NATSInternal.Services.Dtos;

public class SupplyUpsertRequestDto
    : IProductEngageableUpsertRequestDto<SupplyItemRequestDto, SupplyPhotoRequestDto>
{
    public DateTime? SuppliedDateTime { get; set; }
    public long ShipmentFee { get; set; }
    public string Note { get; set; }
    public string UpdateReason { get; set; }
    public List<SupplyItemRequestDto> Items { get; set; }
    public List<SupplyPhotoRequestDto> Photos { get; set; }
    public string UpdatedReason { get; set; }

    public DateTime? StatsDateTime
    {
        get => SuppliedDateTime;
        set => SuppliedDateTime = value;
    }

    public void TransformValues()
    {
        Note = Note?.ToNullIfEmpty();
        UpdateReason = UpdateReason?.ToNullIfEmpty();
        Items.ForEach(i => i.TransformValues());
        Photos.ForEach(p => p.TransformValues());
    }
}
