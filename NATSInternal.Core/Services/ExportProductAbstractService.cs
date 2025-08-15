namespace NATSInternal.Core.Services;

/// <summary>
/// An abstract service to handle financial engagement related operations.
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
/// <typeparam name="TUpsertRequestDto">
/// The type of the request DTO used in the upserting (create or update) operations.
/// </typeparam>
/// <typeparam name="TItemRequestDto">
/// The type of the item request DTO, associated to the
/// <typeparamref name="TUpsertRequestDto"/> DTO.
/// </typeparam>
/// <typeparam name="TPhotoRequestDto">
/// The type of the photo request DTO, associated to the
/// <typeparamref name="TUpsertRequestDto"/> DTO.
/// </typeparam>
/// <typeparam name="TListResponseDto">
/// The type of the response DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TBasicResponseDto">
/// The type of the response DTO, containing the basic information of the
/// <typeparamref name="T"/> entity in the list retriving operation as individual items.
/// </typeparam>
/// <typeparam name="TDetailResponseDto">
/// The type of the response DTO, containing the details of the <typeparamref name="T"/> entity
/// in the detail retriving operation.
/// </typeparam>
/// <typeparam name="TItemResponseDto">
/// The type of the item response DTO, associated to the
/// <typeparamref name="TItemResponseDto"/> DTO in the detail retrieving operation.
/// </typeparam>
/// <typeparam name="TPhotoResponseDto">
/// The type of the photo response DTO, associated to the
/// <typeparamref name="TItemResponseDto"/> DTO in the detail retrieving operation.
/// </typeparam>
/// <typeparam name="TUpdateHistoryResponseDto">
/// The type of the update history response dto, containing the data of the updating history
/// entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TItemUpdateHistoryDataDto">
/// The type of the DTO, containing the data of the updating history item entity associated to
/// the <typeparamref name="TUpdateHistoryResponseDto"/> entity.
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
internal abstract class ExportProductAbstractService<
        T,
        TItem,
        TPhoto,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TItemRequestDto,
        TPhotoRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TItemResponseDto,
        TPhotoResponseDto,
        TUpdateHistoryResponseDto,
        TItemUpdateHistoryDataDto,
        TUpdateHistoryDataDto,
        TCreatingAuthorizationResponseDto,
        TExistingAuthorizationResponseDto>
    : HasProductAbstractService<
        T,
        TItem,
        TPhoto,
        TUpdateHistory,
        TListRequestDto,
        TItemRequestDto,
        TUpdateHistoryDataDto,
        TCreatingAuthorizationResponseDto,
        TExistingAuthorizationResponseDto>
    where T : class, IExportProductEntity<T, TItem, TPhoto, TUpdateHistory>, new()
    where TItem : class, IExportProductItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto : IProductExportableListRequestDto
    where TUpsertRequestDto : IProductExportableUpsertRequestDto<
        TItemRequestDto,
        TPhotoRequestDto>
    where TItemRequestDto : IProductExportableItemRequestDto
    where TPhotoRequestDto : IPhotoRequestDto
    where TListResponseDto :
        IHasStatsResponseDto<
            TBasicResponseDto,
            TExistingAuthorizationResponseDto>,
        new()
    where TBasicResponseDto :
        class,
        IHasStatsBasicResponseDto<TExistingAuthorizationResponseDto>
    where TDetailResponseDto : IHasProductDetailResponseDto<
        TItemResponseDto,
        TPhotoResponseDto,
        TUpdateHistoryResponseDto,
        TItemUpdateHistoryDataDto,
        TExistingAuthorizationResponseDto>
    where TItemResponseDto : IHasProductItemResponseDto
    where TPhotoResponseDto : IPhotoResponseDto
    where TUpdateHistoryResponseDto : IProductExportableUpdateHistoryResponseDto<
        TItemUpdateHistoryDataDto>
    where TItemUpdateHistoryDataDto : IProductExportableItemUpdateHistoryDataDto
    where TCreatingAuthorizationResponseDto : class, IHasStatsCreatingAuthorizationResponseDto, new()
    where TExistingAuthorizationResponseDto : IHasStatsExistingAuthorizationResponseDto, new()
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IMultiplePhotosService<T, TPhoto> _photoService;
    private readonly IStatsInternalService _statsService;

    protected ExportProductAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IMultiplePhotosService<T, TPhoto> photoService,
            IStatsInternalService statsService)
        : base(context, authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
        _photoService = photoService;
        _statsService = statsService;
    }

    /// <summary>
    /// Creates a new entity based on the specified request data.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an
    /// <see cref="int"/>, representing the id of the new entity.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to perform the
    /// operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the customer with the id specified by the property <c>CustomerId</c> in the
    /// <c>requestDto</c> doesn't exist or has already been deleted.
    /// </exception>
    public virtual async Task<int> CreateAsync(TUpsertRequestDto requestDto)
    {
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine stats datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenCreating())
            {
                throw new AuthorizationException();
            }

            // The stats datetime is valid, assign it to the new entity.
            statsDateTime = requestDto.StatsDateTime.Value;
        }

        // Initialize the entity.
        T entity = new T
        {
            StatsDateTime = statsDateTime,
            Note = requestDto.Note,
            CustomerId = requestDto.CustomerId,
            CreatedUserId = _authorizationService.GetUserId(),
            Items = new List<TItem>(),
            Photos = new List<TPhoto>()
        };
        GetRepository(_context).Add(entity);

        // Initialize the items entities.
        await CreateItemsAsync(entity.Items, requestDto.Items);

        // Initialize photos.
        if (requestDto.Photos != null)
        {
            await _photoService.CreateMultipleAsync(entity, requestDto.Photos);
        }

        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();

            // The entity can be created successfully.
            // Increment the stats.
            await AdjustStatsAsync(entity, _statsService, true);

            // Commit the transaction, finish the operation.
            await transaction.CommitAsync();
            return entity.Id;
        }
        catch (DbUpdateException exception)
        {
            // Remove all the created photos.
            foreach (TPhoto photo in entity.Photos)
            {
                _photoService.Delete(photo.Url);
            }

            // Handle the concurency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle the operation exception and convert to the appropriate error.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                HandleCreateOrUpdateException(requestDto, exceptionHandler);
            }
            throw;
        }
    }

    /// <summary>
    /// Updates an existing entity based on the specified request data.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the entity to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the entity to be updated.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the entity with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to perform the
    /// operation.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the customer with the id specified by the property <c>CustomerId</c> in
    /// the argument for the <paramref name="requestDto"/> argument doesn't exist or has
    /// already been deleted.
    /// </exception>
    public async Task UpdateAsync(int id, TUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        T entity = await GetRepository(_context)
            .Include(o => o.CreatedUser)
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Include(o => o.Photos)
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new NotFoundException(
                nameof(entity),
                nameof(id),
                id.ToString());

        // Check if the current user has permission to edit this entity.
        if (!CanEdit(entity))
        {
            throw new AuthorizationException();
        }

        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Decrement the old stats for this entity.
        await AdjustStatsAsync(entity, _statsService, false);

        // Store the current data as old data for update history.
        TUpdateHistoryDataDto oldData = InitializeUpdateHistoryDataDto(entity);

        // Handle the new entityed datetime when the request specifies it.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenEditing(entity))
            {
                throw new AuthorizationException();
            }

            // Assign the new StatsDateTime value only if it's different from the old one.
            if (requestDto.StatsDateTime.Value != entity.StatsDateTime)
            {
                // Validate the specified StatsDateTime value from the request.
                try
                {
                    _statsService.ValidateStatsDateTime<T, TUpdateHistory>(
                        entity,
                        requestDto.StatsDateTime.Value);
                    entity.StatsDateTime = requestDto.StatsDateTime.Value;
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.StatsDateTime);
                    throw new OperationException(
                        nameof(requestDto.StatsDateTime),
                        errorMessage);
                }
            }
        }

        // Update entity properties.
        entity.Note = requestDto.Note;

        // Update entity items.
        await UpdateItemsAsync(entity.Items, requestDto.Items);

        // Update photos.
        List<string> urlsToBeDeletedWhenSucceeds = new List<string>();
        List<string> urlsToBeDeletedWhenFails = new List<string>();
        if (requestDto.Photos != null)
        {
            (List<string>, List<string>) photoUpdateResults = await _photoService
                .UpdateMultipleAsync(entity, requestDto.Photos);
            urlsToBeDeletedWhenSucceeds.AddRange(photoUpdateResults.Item1);
            urlsToBeDeletedWhenFails.AddRange(photoUpdateResults.Item2);
        }

        // Store new data for update history logging.
        TUpdateHistoryDataDto newData = InitializeUpdateHistoryDataDto(entity);

        // Log update history.
        LogUpdateHistory(entity, oldData, newData, requestDto.UpdatedReason);

        // Save changes and handle errors.
        try
        {
            // Save all modifications.
            await _context.SaveChangesAsync();

            // The entity can be updated successfully, add the new the stats.
            await AdjustStatsAsync(entity, _statsService, true);

            // Delete photo files which have been specified.
            foreach (string url in urlsToBeDeletedWhenSucceeds)
            {
                _photoService.Delete(url);
            }

            // Commit the trasaction and finish the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Undo all the created photos.
            foreach (string url in urlsToBeDeletedWhenFails)
            {
                _photoService.Delete(url);
            }

            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handling the exception in the foreseen cases.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                HandleCreateOrUpdateException(requestDto, exceptionHandler);
            }
            throw;
        }
    }

    /// <summary>
    /// Deletes a specific entity by its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the entity to be deleted.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the entity with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to perform the
    /// operation.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the entity's deletion is restricted due to the existence of some related
    /// data.
    /// </exception>
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        T entity = await GetRepository(_context)
            .Include(e => e.Items).ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new NotFoundException(
                typeof(T).Name,
                nameof(id),
                id.ToString());

        // Check if the current user has permission to delete the entity.
        if (!CanDelete(entity))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Delete all the items.
        DeleteItems(entity.Items, GetItemRepository, ProductEngagementType.Export);

        // Delete the entity.
        GetRepository(_context).Remove(entity);

        try
        {
            await _context.SaveChangesAsync();

            // The entity has been deleted successfully, decrement the stats.
            await AdjustStatsAsync(entity, _statsService, false);

            // Commit the transaction and finishing the operations.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle operation exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                if (exceptionHandler.IsDeleteOrUpdateRestricted)
                {
                    // Soft delete when there are any other related entities which are
                    // restricted to be deleted.
                    entity.IsDeleted = true;

                    // Save changes.
                    await _context.SaveChangesAsync();

                    // The entity has been deleted successfully, decrement the stats.
                    await AdjustStatsAsync(entity, _statsService, false);

                    // Commit the transaction and finishing the operations.
                    await transaction.CommitAsync();
                }
            }

            throw;
        }
    }

    /// <inheritdoc />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> items = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Amount),
                DisplayName = DisplayNames.Amount
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.StatsDateTime),
                DisplayName = DisplayNames.StatsDateTime
            }
        };
        

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = items,
            DefaultFieldName = items
                .Single(i => i.Name == nameof(OrderByFieldOption.StatsDateTime))
                .Name,
            DefaultAscending = false
        };
    }

    /// <summary>
    /// Gets a list of entities and month-year options, based on the specified filtering,
    /// sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> which representing the asynchronous operation, which result is
    /// a DTO, containing a list of entities and the additional information for pagination.
    /// </returns>
    protected async Task<EntityListDto<T>> GetListOfEntitiesAsync(
            TListRequestDto requestDto)
    {
        // Initialize query.
        IQueryable<T> query = GetRepository(_context)
            .Include(t => t.Customer)
            .Include(t => t.Items)
            .Include(t => t.Photos);

        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByField
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.Amount):
                query = sortingByAscending
                    ? query.OrderBy(T.AmountAfterVatExpression).ThenBy(t => t.StatsDateTime)
                    : query.OrderByDescending(T.AmountAfterVatExpression)
                        .ThenByDescending(t => t.StatsDateTime);
                break;
            case nameof(OrderByFieldOption.StatsDateTime):
                query = sortingByAscending
                    ? query.OrderBy(t => t.StatsDateTime)
                        .ThenBy(T.AmountAfterVatExpression)
                    : query.OrderByDescending(t => t.StatsDateTime)
                        .ThenBy(T.AmountAfterVatExpression);
                break;
        }

        // Filter by customer id if specified.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(e => e.CustomerId == requestDto.CustomerId);
        }

        return await base.GetListOfEntitiesAsync(query, requestDto);
    }

    /// <inheritdoc />
    protected override sealed async Task<EntityListDto<T>> GetListOfEntitiesAsync(
            IQueryable<T> query,
            TListRequestDto requestDto)
    {
        return await base.GetListOfEntitiesAsync(query, requestDto);
    }

    /// <summary>
    /// Retrieves an entity based on the specified id.
    /// </summary>
    /// <remarks>
    /// The related update history entities will be included if the requesting user has enough
    /// permission to access them.
    /// </remarks>
    /// <param name="id">
    /// The id of the entity to retrieve.
    /// </param>
    /// <returns>
    /// An instance of the <typeparamref name="T"/> entity with the specified id.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the entity with the specified id doesn't exist or has already been deleted.
    /// </exception>
    protected async Task<T> GetEntityAsync(int id)
    {
        // Initialize query.
        IQueryable<T> query = InitializeDetailQuery();

        return await base.GetEntityAsync(query, id);
    }

    /// <summary>
    /// Initialize query which includes all related entities for details retrieving operation.
    /// </summary>
    /// <returns>An instance of the query to retrieve the details.</returns>
    protected virtual IQueryable<T> InitializeDetailQuery()
    {
        return GetRepository(_context)
            .Include(e => e.Customer)
            .Include(e => e.Items).ThenInclude(i => i.Product)
            .Include(e => e.Photos)
            .Include(e => e.CreatedUser).ThenInclude(u => u.Roles);
    }

    /// <summary>
    /// Provides the custom logic to assign the data from the specified DTO into the
    /// initialized entity properties, used in the creating operation.
    /// </summary>
    /// <param name="entity">
    /// The entity of which the properties are to be assigned.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the creating operation.
    /// </param>
    protected virtual void AssignNewEntityProperties(T entity, TUpsertRequestDto requestDto)
    {
    }

    /// <summary>
    /// Provides the custom logic to assign the data from the specified DTO into the
    /// existing entity properties, used in the updating operation.
    /// </summary>
    /// <param name="entity">
    /// The entity of which the properties are to be assigned.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the updating operation.
    /// </param>
    protected virtual void AssignExistingEntityProperty(T entity, TUpsertRequestDto requestDto)
    {
    }

    /// <summary>
    /// Creates new product engagement items, based on the specified items' collection and
    /// items data.
    /// </summary>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> where <c>T</c> is <c>TItemRequestDto</c>, containing the data
    /// for the engagement operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected async Task CreateItemsAsync(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos)
    {
        await CreateItemsAsync(
            itemEntities,
            requestDtos,
            ProductEngagementType.Export,
            (item, itemRequestDto) => item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit);
    }

    /// <summary>
    /// Updates the existing product engagement items and creates new product engagement items
    /// (if specified), based on the specified items' collection and items data.
    /// </summary>
    /// <param name="itemEntities">
    /// A collection of item entities that act as the connection with the products.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> where <c>T</c> is <c>TItemRequestDto</c>, containing the data
    /// for the engagement operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected async Task UpdateItemsAsync(
            ICollection<TItem> itemEntities,
            List<TItemRequestDto> requestDtos)
    {
        await UpdateItemsAsync(
            itemEntities,
            requestDtos,
            ProductEngagementType.Export,
            (item, itemRequestDto) => item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit,
            (item, itemRequestDto) => item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit);
    }

    /// <summary>
    /// Handles the exception thrown by the database when saving during the updating operation
    /// and converts it into the corresponding exception for response.
    /// </summary>
    /// <remarks>
    /// The exception which indicates that the <c>CustomerId</c> and <c>CreatedUserId</c>
    /// foreign key is not found, has already been handled. Provide the custom handling logic
    /// for the other properties if needed.
    /// </remarks>
    /// <param name="requestDto">
    /// A DTO containing the data for the operation.
    /// </param>
    /// <param name="handler">
    /// The exception handler that captured the exception and provides the information of the
    /// exception.
    /// </param>
    /// <exception cref="NotFoundException">
    /// Throws when the customer with the id specified by the value of the <c>CustomerId</c>
    /// in the request DTO doesn't exist or has already been deleted.
    /// </exception>
    private static void HandleCreateOrUpdateException(
            TUpsertRequestDto requestDto,
            SqlExceptionHandler handler)
    {
        if (handler.IsForeignKeyNotFound)
        {
            string violatedFieldName = handler.ViolatedFieldName;
            if (violatedFieldName == GetPropertyName<T>(e => e.CustomerId))
            {
                throw new NotFoundException(
                    nameof(Customer),
                    nameof(Customer.Id),
                    requestDto.CustomerId.ToString());
            }
            else if (violatedFieldName == GetPropertyName<T>(e => e.CreatedUserId))
            {
                throw new ConcurrencyException();
            }
        }
    }

    /// <summary>
    /// Initializes an update history data DTO, containing the data of the specified entity
    /// at the called time, used for storing the data before and after modifications in the
    /// updating operation.
    /// </summary>
    /// <param name="entity">
    /// The entity which data is to be stored.
    /// </param>
    /// <returns>
    /// The intialized DTO.
    /// </returns>
    protected abstract TUpdateHistoryDataDto InitializeUpdateHistoryDataDto(T entity);

    /// <summary>
    /// Increments the statistics amounts with the <typeparamref name="T"/> entity's amounts.
    /// </summary>
    /// <param name="entity">
    /// The instance of the entity with which the associated statistics is to be incremented.
    /// </param>
    /// <param name="statsService">
    /// A provided service for stats operations.
    /// </param>
    /// <param name="isIncrementing">
    /// <c>true</c> to indicate that the stats should be incremented (add new stats).
    /// Otherwise, <c>false</c> to indicate that the stats should be decremented (revert the
    /// old stats).
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected abstract Task AdjustStatsAsync(
            T entity,
            IStatsInternalService statsService,
            bool isIncrementing);
}
