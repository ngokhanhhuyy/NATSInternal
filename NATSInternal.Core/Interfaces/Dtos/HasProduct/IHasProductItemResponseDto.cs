namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasProductItemResponseDto
{
    int Id { get; set; }
    long ProductAmountPerUnit { get; }
    int Quantity { get; }
    ProductBasicResponseDto Product { get; }
}