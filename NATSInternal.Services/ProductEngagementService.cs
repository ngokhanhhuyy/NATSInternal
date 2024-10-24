namespace NATSInternal.Services;

/// <inheritdoc />
internal class ProductEngagementService<TItem, TPhoto, TUpdateHistory>
    : IProductEngagementService<TItem, TPhoto, TUpdateHistory>
    where TItem : class, IProductEngageableItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    protected readonly DatabaseContext _context;

    public ProductEngagementService(DatabaseContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task CreateItemsAsync<TItemRequestDto>(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType)
        where TItemRequestDto : IProductEngageableItemRequestDto
    {
        // Fetch a list of products which ids are specified in the request.
        List<int> requestedProductIds = requestDtos.Select(i => i.ProductId).ToList();
        List<Product> products = await _context.Products
            .Where(p => requestedProductIds.Contains(p.Id))
            .ToListAsync();

        for (int i = 0; i < requestDtos.Count; i++)
        {
            TItemRequestDto itemRequestDto = requestDtos[i];
            // Get the product with the specified id from pre-fetched list.
            Product product = products.SingleOrDefault(p => p.Id == itemRequestDto.Id);

            // Ensure the product exists.
            if (product == null)
            {
                string errorMessage = ErrorMessages.NotFoundByProperty
                    .ReplaceResourceName(DisplayNames.Product)
                    .ReplacePropertyName(DisplayNames.Id)
                    .ReplaceAttemptedValue(itemRequestDto.ProductId.ToString());
                throw new OperationException($"items[{i}].productId", errorMessage);
            }

            // Validate that in an export operation, the exporting quantity is not greater than
            // the currently available quantity in stock.
            if (engagementType == ProductEngagementType.Export &&
                product.StockingQuantity < itemRequestDto.Quantity)
            {
                const string errorMessage = ErrorMessages.NegativeProductStockingQuantity;
                throw new OperationException($"items[{i}].quantity", errorMessage);
            }

            // Initialize entity.
            TItem item = new TItem
            {
                ProductAmountPerUnit = itemRequestDto.ProductAmountPerUnit,
                Quantity = itemRequestDto.Quantity,
                Product = product
            };

            // Adjust the stocking quantity of the associated product.
            if (engagementType == ProductEngagementType.Import)
            {
                product.StockingQuantity += item.Quantity;
            }
            else
            {
                product.StockingQuantity -= item.Quantity;
            }

            // Add item.
            itemEntities.Add(item);
        }
    }
    
    /// <inheritdoc />
    public async Task UpdateItemsAsync<TItemRequestDto>(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType,
            string itemDisplayName)
        where TItemRequestDto : IProductEngageableItemRequestDto
    {
        // Fetch a list of products for the items which are indicated to be created.
        List<int> productIdsForNewItems = requestDtos
            .Where(i => !i.Id.HasValue)
            .Select(i => i.ProductId)
            .ToList();
        List<Product> productsForNewItems = await _context.Products
            .Where(p => productIdsForNewItems.Contains(p.Id))
            .ToListAsync();

        itemEntities ??= new List<TItem>();
        for (int i = 0; i < requestDtos.Count; i++)
        {
            TItemRequestDto itemRequestDto = requestDtos[i];
            if (itemRequestDto.HasBeenChanged)
            {
                TItem item;

                // Initialize a new entity when the request doesn't have id.
                if (itemRequestDto.Id.HasValue)
                {
                    // Get the entity by the given id and ensure it exists.
                    item = itemEntities.SingleOrDefault(si => si.Id == itemRequestDto.Id);
                    if (item == null)
                    {
                        string errorMessage = ErrorMessages.NotFoundByProperty
                            .ReplaceResourceName(itemDisplayName)
                            .ReplacePropertyName(DisplayNames.Id)
                            .ReplaceAttemptedValue(itemRequestDto.Id.ToString());
                        throw new OperationException($"items[{i}].id", errorMessage);
                    }

                    // Revert the stocking quantity of the product associated to the item.
                    if (engagementType == ProductEngagementType.Import)
                    {
                        item.Product.StockingQuantity += item.Quantity;
                    }
                    else
                    {
                        item.Product.StockingQuantity -= item.Quantity;
                    }

                    // Delete the entity if specified.
                    if (itemRequestDto.HasBeenDeleted)
                    {
                        itemEntities.Remove(item);
                        continue;
                    }

                    item.ProductAmountPerUnit = itemRequestDto.ProductAmountPerUnit;
                    item.Product.StockingQuantity -= item.Quantity;
                    item.Quantity = itemRequestDto.Quantity;
                }
                else
                {
                    // Get the product entity from the pre-fetched list.
                    Product product = productsForNewItems
                        .SingleOrDefault(p => p.Id == itemRequestDto.ProductId);

                    // Ensure the product exists in the database.
                    if (product == null)
                    {
                        string errorMessage = ErrorMessages.NotFoundByProperty
                            .ReplaceResourceName(DisplayNames.Product)
                            .ReplacePropertyName(DisplayNames.Id)
                            .ReplaceAttemptedValue(itemRequestDto.ProductId.ToString());
                        throw new OperationException($"item[{i}].productId", errorMessage);
                    }

                    // Initialize new supply item.
                    item = new TItem
                    {
                        ProductAmountPerUnit = itemRequestDto.ProductAmountPerUnit,
                        Quantity = itemRequestDto.Quantity,
                        ProductId = itemRequestDto.ProductId
                    };
                    itemEntities.Add(item);
                }

                // Adjust product stocking quantity.
                if (engagementType == ProductEngagementType.Import)
                {
                    item.Product.StockingQuantity += item.Quantity;
                }
                else
                {
                    item.Product.StockingQuantity -= item.Quantity;
                }
            }
        }
    }
    
    public void DeleteItems(
            ICollection<TItem> itemEntities,
            Func<DatabaseContext, DbSet<TItem>> repositorySelector,
            ProductEngagementType engagementType)
    {
        foreach (TItem item in itemEntities)
        {
            // Revert the stocking quantity of the product associated to the item.
            if (engagementType == ProductEngagementType.Import)
            {
                item.Product.StockingQuantity -= item.Quantity;
                if (item.Product.StockingQuantity < 0)
                {
                    const string errorMessage = ErrorMessages.NegativeProductStockingQuantity;
                    throw new OperationException(errorMessage);
                }
            }
            else
            {
                item.Product.StockingQuantity += item.Quantity;
            }

            // Remove the item.
            itemEntities.Remove(item);
            repositorySelector(_context).Remove(item);
        }
    }
}