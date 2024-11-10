namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasProductItemRequestDto : IRequestDto
{
    int? Id { get; set; }
    long ProductAmountPerUnit { get; set; }
    int Quantity { get; set; }
    int ProductId { get; set; }
    bool HasBeenChanged { get; set; }
    bool HasBeenDeleted { get; set; }
}