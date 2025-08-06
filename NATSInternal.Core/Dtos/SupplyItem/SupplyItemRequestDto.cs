namespace NATSInternal.Core.Dtos;

public class SupplyItemRequestDto : IHasProductItemRequestDto
{
    public int? Id { get; set; }
    public long ProductAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
        Id = Id == 0 ? null : Id;
    }
}
