namespace NATSInternal.Core;

/// <summary>
/// An abstract service to handle operations which interact with the product-engageable
/// entites.
/// </summary>
/// <typeparam name="T">
/// The type of the entity.
/// </typeparam>
/// <typeparam name="TItem">
/// The type of the item entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TPhoto">
/// The type of the photo entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TItemRequestDto">
/// The type of the item request DTO.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the update history data DTO, containing the data of a specific
/// <typeparamref name="T"/> entity instance after each modification, used in the updating
/// operation.
/// </typeparam>
/// <typeparam name="TCreatingAuthorizationResponseDto">
/// The type of response DTO which contains the authorization information when creating a new
/// <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TExistingAuthorizationResponseDto">
/// The type of response DTO which contains the authorization information when updating an
/// existing <typeparamref name="T"/> entity.
/// </typeparam>
internal abstract class HasProductAbstractService<
        T,
        TItem,
        TPhoto,
        TUpdateHistory,
        TListRequestDto,
        TItemRequestDto,
        TUpdateHistoryDataDto,
        TCreatingAuthorizationResponseDto,
        TExistingAuthorizationResponseDto>
    : HasStatsAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpdateHistoryDataDto,
        TCreatingAuthorizationResponseDto,
        TExistingAuthorizationResponseDto>
    where T : class, IHasProductEntity<T, TItem, TPhoto, TUpdateHistory>, new()
    where TItem : class, IHasProductItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto : IHasProductListRequestDto
    where TItemRequestDto : IHasProductItemRequestDto
    where TCreatingAuthorizationResponseDto : class, IHasStatsCreatingAuthorizationResponseDto, new()
    where TExistingAuthorizationResponseDto : IHasStatsExistingAuthorizationResponseDto, new()
{
    private readonly DatabaseContext _context;

    protected HasProductAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService)
        : base(context, authorizationService)
    {
        _context = context;
    }

    /// <inheritdoc />
    protected override async Task<EntityListDto<T>> GetListOfEntitiesAsync(
            IQueryable<T> query,
            TListRequestDto requestDto)
    {
        IQueryable<T> filteredQuery = query;

        if (requestDto.ProductId.HasValue)
        {
            filteredQuery = query
                .Where(e => e.Items.Any(ei => ei.ProductId == requestDto.ProductId));
        }

        return await base.GetListOfEntitiesAsync(filteredQuery, requestDto);
    }

    /// <summary>
    /// Creates new product engagement items, based on the specified items' collection, items
    /// data and engagement type.
    /// </summary>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> where <c>T</c> is <c>TItemRequestDto</c>, containing the data
    /// for the engagement operation.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <param name="itemInitializer">
    /// A function to initialize an instance of the <typeparamref name="TItem"/> entity from
    /// a given request DTO.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected async Task CreateItemsAsync(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType,
            Action<TItem, TItemRequestDto> itemInitializer = null)
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
            Product product = products.SingleOrDefault(p => p.Id == itemRequestDto.ProductId);

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

            itemInitializer?.Invoke(item, itemRequestDto);

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

    /// <summary>
    /// Updates the existing product engagement items and creates new product engagement items
    /// (if specified), based on the specified items' collection, items data and engagement
    /// type.
    /// </summary>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> where <c>T</c> is <c>TItemRequestDto</c>, containing the data
    /// for the engagement operation.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <param name="itemInitializer">
    /// A function to assign the data from the given <typeparamref name="TItemRequestDto"/> DTO
    /// to the newly initialized <typeparamref name="TItem"/> entity when the entity is
    /// created.
    /// </param>
    /// <param name="itemUpdatingAssigner">
    /// A function to assign the data from the given <typeparamref name="TItemRequestDto"/> DTO
    /// to the existing <typeparamref name="TItem"/> entity when updating the entity.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected async Task UpdateItemsAsync(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos,
            ProductEngagementType engagementType,
            Action<TItem, TItemRequestDto> itemInitializer = null,
            Action<TItem, TItemRequestDto> itemUpdatingAssigner = null)
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
                            .ReplaceResourceName(DisplayNames.Get(typeof(TItem).Name))
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
                    itemUpdatingAssigner?.Invoke(item, itemRequestDto);
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
                        Product = product
                    };
                    itemInitializer?.Invoke(item, itemRequestDto);
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

    /// <summary>
    /// Deletes the existing product engagement items from the specified collection and
    /// repository, based on the specified engagement operation type.
    /// </summary>
    /// <param name="itemEntities">
    /// A list of item entities that act as the connection with the products.
    /// </param>
    /// <param name="repositorySelector">
    /// A function that is used to select the <typeparamref name="TItem"/> repository from the given
    /// <see cref="DatabaseContext"/> instance.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected void DeleteItems(
            List<TItem> itemEntities,
            Func<DatabaseContext, DbSet<TItem>> repositorySelector,
            ProductEngagementType engagementType)
    {
        for (int i = itemEntities.Count - 1; i >= 0; i--)
        {
            TItem item = itemEntities[i];

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

    /// <summary>
    /// Gets the item entity repository in the <see cref="DatabaseContext"/> class.
    /// </summary>
    /// <param name="context">
    /// An instance of the injected <see cref="DatabaseContext"/>
    /// </param>
    /// <returns>
    /// The item entity repository.
    /// </returns>
    protected abstract DbSet<TItem> GetItemRepository(DatabaseContext context);
}