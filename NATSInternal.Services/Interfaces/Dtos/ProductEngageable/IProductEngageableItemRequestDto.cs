namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableItemRequestDto
{
    int? Id { get; set; }
    long AmountPerUnit { get; set; }
    int Quantity { get; set; }
    int ProductId { get; set; }
    bool HasBeenChanged { get; set; }
    bool HasBeenDeleted { get; set; }
}