using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Features.Products;
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
        Action<TItemRequestDto, TItemEntity> mapper,
        Action<Stock> stockAdjuster,
        string itemsPropertyName)
    {
        List<TItemEntity> itemEntities = new();
        IEnumerable<int> requestProductIds = itemRequestDtos.Select(i => i.ProductId);
        List<Product> existingProducts = await _context.Products
            .Include(p => p.Stock)
            .Where(p => requestProductIds.Contains(p.Id) && p.DeletedDateTime == null)
            .ToListAsync();

        List<int> processedProductIds = new();

        for (int index = 0; index < itemRequestDtos.Count; index += 1)
        {
            TItemRequestDto itemRequestDto = itemRequestDtos[index];
            
            Product product = existingProducts
                .SingleOrDefault(p => p.Id == itemRequestDto.ProductId)
                ?? throw OperationException.NotFound(
                    new object[] { itemsPropertyName, index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product
                );

            if (processedProductIds.Contains(itemRequestDto.ProductId))
            {
                throw OperationException.Duplicated(
                    new object[] { itemsPropertyName, index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product
                );
            }

            TItemEntity itemEntity = new()
            {
                Quantity = itemRequestDto.Quantity,
                ProductId = itemRequestDto.ProductId
            };

            mapper(itemRequestDto, itemEntity);

            product.Stock ??= new();
            stockAdjuster(product.Stock);
            if (product.Stock.StockingQuantity < 0)
            {
                throw new OperationException(
                    new object[] { itemsPropertyName, index, nameof(itemRequestDto.Quantity) },
                    ErrorMessages.NegativeProductStockingQuantity
                );
            }

            itemEntities.Add(itemEntity);
            processedProductIds.Add(itemRequestDto.ProductId);
        }

        return itemEntities;
    }

    public async Task UpdateItemsAsync(
        List<TItemRequestDto> itemRequestDtos,
        List<TItemEntity> existingItems,
        Action<TItemRequestDto, TItemEntity> mapper,
        Action<Stock> stockAdjuster,
        string itemsPropertyName)
    {
        IEnumerable<int> requestItemIds = itemRequestDtos.Select(i => i.Id).OfType<int>();
        IEnumerable<int> requestProductIds = itemRequestDtos.Select(i => i.ProductId);
        List<Product> existingProducts = await _context.Products
            .Include(p => p.Stock)
            .Where(p => requestProductIds.Contains(p.Id) && p.DeletedDateTime == null)
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
                    new object[] { itemsPropertyName, index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product);
            }
            
            Product product = existingProducts
                .SingleOrDefault(p => p.Id == itemRequestDto.ProductId)
                ?? throw OperationException.NotFound(
                    new object[] { itemsPropertyName, index, nameof(itemRequestDto.ProductId) },
                    DisplayNames.Product
                );

            TItemEntity itemEntity;
            if (!itemRequestDto.Id.HasValue)
            {
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
                        new object[] { itemsPropertyName, index, nameof(itemRequestDto.ProductId) },
                        DisplayNames.Product);
                }

                itemEntity = existingItems
                    .SingleOrDefault(ie => ie.Id == itemRequestDto.Id.Value)
                    ?? throw OperationException.NotFound(
                        new object[] { itemsPropertyName, index, nameof(itemRequestDto.Id) },
                        DisplayNames.SupplyItem);

                processedItemIds.Add(itemEntity.Id);
            }

            mapper(itemRequestDto, itemEntity);
            
            product.Stock ??= new();
            stockAdjuster(product.Stock);
            if (product.Stock.StockingQuantity < 0)
            {
                throw new OperationException(
                    new object[] { itemsPropertyName, index, nameof(itemRequestDto.Quantity) },
                    ErrorMessages.NegativeProductStockingQuantity
                );
            }

            processedProductIds.Add(itemRequestDto.ProductId);
        }
    }

    public void DeleteItemsAsync(List<TItemEntity> existingItems, Action<Stock> stockAdjuster)
    {
        for (int index = 0; index < existingItems.Count; index += 1)
        {
            TItemEntity itemEntity = existingItems[index];
            if (itemEntity.Product?.Stock is not null)
            {
                stockAdjuster(itemEntity.Product.Stock);
                {
                    throw new OperationException(ErrorMessages.NegativeProductStockingQuantity);
                }
            }

            _context.Remove(itemEntity);
        }
    }
    #endregion
}