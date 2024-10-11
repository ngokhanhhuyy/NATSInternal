namespace NATSInternal.Services;

/// <summary>
/// A service to handle product-engagement-related operations.
/// </summary>
/// <typeparam name="TItem">
/// The type of the item entity, which is the connection between the
/// <see cref="IProductEngageableEntity{T, TItem, TPhoto, TUpdateHistory}"/>
/// and the <see cref="TProduct" /> entity.
/// </typeparam>
/// <typeparam name="TPhoto">
/// The type of the photo entity, which is contained by the
/// <see cref="IProductEngageableEntity{T, TItem, TPhoto, TUpdateHistory}"/> entity that
/// contains the <see cref="TItem"/> entities.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity which is associated with the
/// <see cref="IProductEngageableEntity{T, TItem, TPhoto, TUpdateHistory}"/> entity that
/// contains the <see cref="TItem"/> entities.
/// </typeparam>
internal abstract class ProductEngageableAbstractService<
        T,
        TItem,
        TPhoto,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TItemResponseDto,
        TPhotoResponseDto,
        TUpdateHistoryResponseDto,
        TUpdateHistoryDataDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    : FinancialEngageableAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TUpdateHistoryResponseDto,
        TUpdateHistoryDataDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T : class, IProductEngageableEntity<T, TItem, TPhoto, TUpdateHistory>, new()
    where TItem : class, IProductEngageableItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto :
        IFinancialEngageableListRequestDto,
        ICustomerEngageableListRequestDto
    where TUpsertRequestDto : ICustomerEngageableUpsertRequestDto
    where TListResponseDto :
        IFinancialEngageableListResponseDto<
            TBasicResponseDto,
            TAuthorizationResponseDto,
            TListAuthorizationResponseDto>,
        new()
    where TBasicResponseDto :
        class,
        ICustomerEngageableBasicResponseDto<TAuthorizationResponseDto>
    where TDetailResponseDto : IDebtDetailResponseDto<
        TUpdateHistoryResponseDto,
        TAuthorizationResponseDto>
    where TItemResponseDto : IProductEngageableItemResponseDto
    where TPhotoResponseDto : IProductEngageableItemResponseDto
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService<T, TPhoto> _photoService;

    protected ProductEngageableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IPhotoService<T, TPhoto> photoService,
            IMonthYearService<T, TUpdateHistory> monthYearService)
        : base (authorizationService, monthYearService)
    {
        _context = context;
        _photoService = photoService;
    }

    /// <summary>
    /// Retrieves a list of DTOs containing the basic information of the debt instances,
    /// specified filtering, sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="DebtPaymentListRequestDto"/> class, containing the
    /// conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a
    /// <see cref="List{T}"/> of DTOs, containing the results of the operation and the additional
    /// information for pagination.
    /// </returns>
    public virtual async Task<TListResponseDto> GetListAsync(TListRequestDto requestDto)
    {
        return await base.GetListAsync(InitializeListQuery(requestDto), requestDto);
    }

    /// <summary>
    /// Creates new product engagement items, based on the specified items' collection, items
    /// data and engagement type.
    /// </summary>
    /// <typeparam name="TItemRequestDto">
    /// The type of the item request DTOs that contains the data for the engagement operation.
    /// </typeparam>
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
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected async Task CreateItemsAsync<TItemRequestDto>(
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
            itemEntities.Add(item);
        }
    }
    
    /// <summary>
    /// Updates the existing product engagement items and creates new product engagement items
    /// (if specified), based on the specified items' collection, items data and engagement
    /// type.
    /// </summary>
    /// <typeparam name="TItemRequestDto">
    /// The type of the item request DTOs that contains the data for the engagement operation.
    /// </typeparam>
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
    /// <param name="itemDisplayName">
    /// A <see cref="string"/> value represent the display name of the item entity, used in
    /// the error message when the operation fails.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected async Task UpdateItemsAsync<TItemRequestDto>(
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

                    item.AmountPerUnit = itemRequestDto.AmountPerUnit;
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
                        AmountPerUnit = itemRequestDto.AmountPerUnit,
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
    
    /// <summary>
    /// Deletes the existing product engagement items from the specified collection and
    /// repository, based on the specified engagement operation type.
    /// </summary>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="repositorySelector">
    /// A function that is used to select the <see cref="TItem"/> repository from the given
    /// <see cref="DatabaseContext"/> instance.
    /// </param>
    /// <param name="engagementType">
    /// The type of the engagement operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected void DeleteItems(
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

    /// <summary>
    /// Initializes the query for list retrieving operation, based on the filtering, sorting
    /// and paginating conditions specified in the request DTO.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the conditions for the results.
    /// </param>
    /// <returns>
    /// A query instance used to perform the list retrieving operation.
    /// </returns>
    protected virtual IQueryable<T> InitializeListQuery(TListRequestDto requestDto)
    {
        IQueryable<T> query = GetRepository(_context)
            .Include(entity => entity.Customer);

        // Sort by the specified direction and field.
        query = SortListQuery(query, requestDto);

        // Filter by month and year if specified.
        if (!requestDto.IgnoreMonthYear)
        {
            DateTime startingDateTime = new DateTime(requestDto.Year, requestDto.Month, 1);
            DateTime endingDateTime = startingDateTime.AddMonths(1);
            query = FilterByMonthYearListQuery(query, startingDateTime, endingDateTime);
        }

        // Filter by user id if specified.
        if (requestDto.CreatedUserId.HasValue)
        {
            query = query.Where(o => o.CreatedUserId == requestDto.CreatedUserId);
        }

        // Filter by customer id if specified.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(o => o.CustomerId == requestDto.CustomerId);
        }

        // Filter by not being soft deleted.
        query = query.Where(o => !o.IsDeleted);

        return query;
    }