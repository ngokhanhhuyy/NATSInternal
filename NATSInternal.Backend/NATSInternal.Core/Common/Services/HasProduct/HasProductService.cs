using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Common.Services;

internal class HasProductService<TItemRequestDto, TItemEntity> : IHasProductService<TItemRequestDto, TItemEntity>
    where TItemRequestDto : IHasProductItemUpsertRequestDto
    where TItemEntity : class, IHasProductItemEntity, new()
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public HasProductService(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<List<TItemEntity>> CreateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        Action<TItemRequestDto, TItemEntity> mapper)
    {
        List<TItemEntity> itemEntities = new();
        IEnumerable<int> requestProductIds = itemRequestDtos.Select(i => i.ProductId);
        List<int> existingProductIds = await _context.Products
            .Where(p => requestProductIds.Contains(p.Id) && p.DeletedDateTime == null)
            .Select(p => p.Id)
            .ToListAsync();

        List<int> processedProductIds = new();

        for (int index = 0; index < itemRequestDtos.Count; index += 1)
        {
            TItemRequestDto itemRequestDto = itemRequestDtos[index];
            if (!existingProductIds.Contains(itemRequestDto.ProductId))
            {
                throw OperationException.NotFound(
                    new object[] { nameof(itemRequestDtos), index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product
                );
            }

            if (processedProductIds.Contains(itemRequestDto.ProductId))
            {
                throw OperationException.Duplicated(
                    new object[] { nameof(itemRequestDtos), index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product
                );
            }

            TItemEntity itemEntity = new()
            {
                Quantity = itemRequestDto.Quantity,
                ProductId = itemRequestDto.ProductId
            };

            mapper(itemRequestDto, itemEntity);
            itemEntities.Add(itemEntity);
            processedProductIds.Add(itemRequestDto.ProductId);
        }

        return itemEntities;
    }

    public async Task UpdateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        List<TItemEntity> existingItems,
        Action<TItemRequestDto, TItemEntity> mapper)
    {
        IEnumerable<int> requestItemIds = itemRequestDtos.Select(i => i.Id).OfType<int>();
        IEnumerable<int> requestProductIds = itemRequestDtos.Select(i => i.ProductId);
        List<int> existingProductIds = await _context.Products
            .Where(p => requestProductIds.Contains(p.Id) && p.DeletedDateTime == null)
            .Select(p => p.Id)
            .ToListAsync();

        List<int> processedProductIds = new();
        List<int> processedItemIds = new();

        IEnumerable<TItemEntity> itemEntitiesToBeDeleted = existingItems.Where(i => !requestItemIds.Contains(i.Id));
        foreach (TItemEntity itemEntity in itemEntitiesToBeDeleted)
        {
            existingItems.Remove(itemEntity);
            processedProductIds.Add(itemEntity.ProductId);
        }

        for (int index = 0; index < itemRequestDtos.Count; index += 1)
        {
            TItemRequestDto itemRequestDto = itemRequestDtos[index];

            if (processedProductIds.Contains(itemRequestDto.ProductId))
            {
                throw OperationException.Duplicated(
                    new object[] { nameof(itemRequestDtos), index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product);
            }

            TItemEntity itemEntity;
            if (!itemRequestDto.Id.HasValue)
            {
                if (!existingProductIds.Contains(itemRequestDto.ProductId))
                {
                    throw OperationException.NotFound(
                        new object[] { nameof(itemRequestDtos), index, nameof(itemRequestDto.ProductId) },
                        DisplayNames.Product);
                }

                itemEntity = new()
                {
                    Quantity = itemRequestDto.Quantity,
                    ProductId = itemRequestDto.ProductId
                };

                existingItems.Add(itemEntity);
            }
            else
            {
                if (processedItemIds.Contains(itemRequestDto.Id.Value))
                {
                    throw OperationException.Duplicated(
                        new object[] { nameof(itemRequestDtos), index, nameof(itemRequestDto.ProductId) },
                        DisplayNames.Product);
                }

                itemEntity = existingItems
                    .SingleOrDefault(ie => ie.Id == itemRequestDto.Id.Value)
                    ?? throw OperationException.NotFound(
                        new object[] { nameof(itemRequestDtos), index, nameof(itemRequestDto.Id) },
                        DisplayNames.SupplyItem);

                processedItemIds.Add(itemEntity.Id);
            }

            mapper(itemRequestDto, itemEntity);

            processedProductIds.Add(itemRequestDto.ProductId);
        }
    }
    #endregion
}