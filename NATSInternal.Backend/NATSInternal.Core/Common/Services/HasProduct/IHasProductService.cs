using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Entities;

namespace NATSInternal.Core.Common.Services;

internal interface IHasProductService<TItemRequestDto, TItemEntity>
    where TItemRequestDto : IHasProductItemUpsertRequestDto
    where TItemEntity : class, IHasProductItemEntity, new()
{
    #region Methods
    Task<List<TItemEntity>> CreateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        Action<TItemRequestDto, TItemEntity> mapper);
        
    Task UpdateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        List<TItemEntity> existingItems,
        Action<TItemRequestDto, TItemEntity> mapper);
    #endregion
}