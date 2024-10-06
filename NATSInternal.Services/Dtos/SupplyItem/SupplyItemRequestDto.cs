namespace NATSInternal.Services.Dtos;

public class SupplyItemRequestDto : IRequestDto
{
    public int? Id { get; set; }
    public long Amount { get; set; }
    public int SuppliedQuantity { get; set; }
    public int ProductId { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
        Id = Id.HasValue && Id.Value == 0 ? null : Id;
    }
}
