namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductEngageableItemRequestDto : IRequestDto
{
    int? Id { get; set; }
    long AmountPerUnit { get; set; }
    int Quantity { get; set; }
    int ProductId { get; set; }
    bool HasBeenChanged { get; set; }
    bool HasBeenDeleted { get; set; }
}