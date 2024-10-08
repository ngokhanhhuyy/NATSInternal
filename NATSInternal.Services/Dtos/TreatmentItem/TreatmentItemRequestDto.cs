namespace NATSInternal.Services.Dtos;

public class TreatmentItemRequestDto : IProductEngageableItemRequestDto
{
    public int? Id { get; set; }
    public long AmountPerUnit { get; set; }
    public int VatAmount { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
        Id = Id == 0 ? null : Id;
    }
}
