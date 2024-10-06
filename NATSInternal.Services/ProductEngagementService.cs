namespace NATSInternal.Services;

internal class ProductEngagementService<T, TCustomer, TItem, TProduct, TPhoto, TUser, TUpdateHistory>
    : IProductEngagementService<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>
    where T : class, IProductEngageableEntity<T, TItem, TProduct, TPhoto, TUser, TUpdateHistory>, new()
    where TCustomer : class, ICustomerEntity<TCustomer, TUser>, new()
    where TItem : class, IProductEngageableItemEntity<TItem, TProduct>, new()
    where TProduct : class, IProductEntity<TProduct>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUser : class, IUserEntity<TUser>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory, TUser>, new()
{
    protected readonly IDatabaseContext<TUser, TCustomer, TProduct> _context;

    public ProductEngagementService(IDatabaseContext<TUser, TCustomer, TProduct> context)
    {
        _context = context;
    }
    
    public async Task CreateItemsAsync<TItemRequestDto>(
            T entity,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType)
        where TItemRequestDto : IProductEngageableItemRequestDto
    {
        // Fetch a list of products which ids are specified in the request.
        List<int> requestedProductIds = requestDtos.Select(i => i.ProductId).ToList();
        List<TProduct> products = await _context.Products
            .Where(p => requestedProductIds.Contains(p.Id))
            .ToListAsync();

        for (int i = 0; i < requestDtos.Count; i++)
        {
            TItemRequestDto itemRequestDto = requestDtos[i];
            // Get the product with the specified id from pre-fetched list.
            TProduct product = products.SingleOrDefault(p => p.Id == itemRequestDto.Id);

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
                AmountPerUnit = itemRequestDto.AmountPerUnit,
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
            entity.Items.Add(item);
        }
    }
    
    public async Task UpdateItemsAsync<TItemRequestDto>(
            T entity,
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
        List<TProduct> productsForNewItems = await _context.Products
            .Where(p => productIdsForNewItems.Contains(p.Id))
            .ToListAsync();

        entity.Items ??= new List<TItem>();
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
                    item = entity.Items.SingleOrDefault(si => si.Id == itemRequestDto.Id);
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
                        entity.Items.Remove(item);
                        continue;
                    }

                    item.AmountPerUnit = itemRequestDto.AmountPerUnit;
                    item.Product.StockingQuantity -= item.Quantity;
                    item.Quantity = itemRequestDto.Quantity;
                }
                else
                {
                    // Get the product entity from the pre-fetched list.
                    TProduct product = productsForNewItems
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
                        AmountPerUnit = itemRequestDto.AmountPerUnit,
                        Quantity = itemRequestDto.Quantity,
                        ProductId = itemRequestDto.ProductId
                    };
                    entity.Items.Add(item);
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
            T entity,
            DbSet<TItem> itemRepository,
            ProductEngagementType engagementType)
    {
        foreach (TItem item in entity.Items)
        {
            // Revert the stocking quantity of the product associated to the item.
            item.Product.StockingQuantity -= item.Quantity;

            // Remove the item.
            itemRepository.Remove(item);
        }
    }
}