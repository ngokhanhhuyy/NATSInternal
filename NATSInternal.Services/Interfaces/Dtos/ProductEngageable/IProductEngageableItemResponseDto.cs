namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableItemResponseDto
{
    int Id { get; set; }
    long ProductAmountPerUnit { get; }
    int Quantity { get; }
    ProductBasicResponseDto Product { get; }
}