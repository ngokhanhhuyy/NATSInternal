using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Features.Products;

namespace NATSInternal.Core.Common.Services;

internal interface IHasProductService<TItemRequestDto, TItemEntity>
    where TItemRequestDto : IHasProductItemUpsertRequestDto
    where TItemEntity : class, IHasProductItemEntity, new()
{
    #region Methods
    Task<List<TItemEntity>> CreateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        Action<TItemRequestDto, TItemEntity> mapper,
        Action<Product> stockingQuantityAdjuster,
        string itemsPropertyName);
        
    Task UpdateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        List<TItemEntity> existingItems,
        Action<TItemRequestDto, TItemEntity> mapper,
        Action<Product> stockingQuantityAdjuster,
        string itemsPropertyName);

    void DeleteItemsAsync(List<TItemEntity> existingItems, Action<Product> stockingQuantityAdjuster);
    #endregion
}
