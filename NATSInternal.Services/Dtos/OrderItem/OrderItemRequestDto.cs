namespace NATSInternal.Services.Dtos;

public class OrderItemRequestDto : IProductEngageableItemRequestDto
{
    public int? Id { get; set; }
    public long AmountPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public int ProductId { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }
        
    public void TransformValues()
    {
        if (Id == 0)
        {
            Id = null;
        }
    }
}