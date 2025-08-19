namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasProductItemResponseDto
{
    #region Properties
    Guid Id { get; set; }
    long ProductAmountPerUnit { get; }
    int Quantity { get; }
    ProductBasicResponseDto Product { get; }
    #endregion
}