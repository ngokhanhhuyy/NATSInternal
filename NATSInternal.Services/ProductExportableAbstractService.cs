namespace NATSInternal.Services;

/// <summary>
/// An abstract service to handle financial engagement related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <see cref="T"/> entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TListResponseDto">
/// The type of the response DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TBasicResponseDto">
/// The type of the response DTO, containing the basic information of the <see cref="T"/>
/// entity in the list retriving operation as individual items.
/// </typeparam>
/// <typeparam name="TDetailResponseDto">
/// The type of the response DTO, containing the details of the <see cref="T"/> entity in the
/// detail retriving operation.
/// </typeparam>
/// <typeparam name="TUpdateHistoryResponseDto">
/// The type of the update history response dto, containing the data of the updating history
/// entity associated to the <see cref="T"/> entity.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the update history data DTO, containing the data of a specific <see cref="T"/>
/// entity instance after each modification, used in the updating operation.
/// </typeparam>
/// <typeparam name="TListAuthorizationResponseDto">
/// The type of the response DTO, containing the authorization information for the
/// <see cref="TListResponseDto"/> DTO in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TAuthorizationResponseDto">
/// The type of the response DTO, containing the authorization information for the
/// <see cref="TBasicResponseDto"/> and <see cref="TDetailResponseDto"/> DTOs, used in the
/// list retrieving and detail retrieving operations.
/// </typeparam>
internal abstract class ProductExportableAbstractService<
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
        TUpdateHistoryDataDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    : ProductEngageableAbstractService<
        T,
        TItem,
        TPhoto,
        TUpdateHistory,
        TListRequestDto,
        TItemRequestDto,
        TUpdateHistoryDataDto>
    where T : class, IProductExportableEntity<T, TItem, TPhoto, TUpdateHistory>, new()
    where TItem : class, IProductExportableItemEntity<TItem>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto : IProductExportableListRequestDto
    where TUpsertRequestDto : IProductExportableUpsertRequestDto<
        TItemRequestDto,
        TPhotoRequestDto>
    where TItemRequestDto : IProductExportableItemRequestDto
    where TPhotoRequestDto : IPhotoRequestDto
    where TListResponseDto :
        IFinancialEngageableListResponseDto<
            TBasicResponseDto,
            TAuthorizationResponseDto,
            TListAuthorizationResponseDto>,
        new()
    where TBasicResponseDto :
        class,
        IFinancialEngageableBasicResponseDto<TAuthorizationResponseDto>
    where TDetailResponseDto : IProductEngageableDetailResponseDto<
        TItemResponseDto,
        TPhotoResponseDto,
        TUpdateHistoryResponseDto,
        TAuthorizationResponseDto>
    where TItemResponseDto : IProductEngageableItemResponseDto
    where TPhotoResponseDto : IPhotoResponseDto
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IPhotoService<T, TPhoto> _photoService;
    private readonly IStatsInternalService<T, TUpdateHistory> _statsService;

    protected ProductExportableAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IPhotoService<T, TPhoto> photoService,
            IStatsInternalService<T, TUpdateHistory> statsService)
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
            // Check if the current user has permission to specify the stats datetime.
            if (!CanSetStatsDateTime(_authorizationService))
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
    /// <exception cref="ResourceNotFoundException">
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
    /// the argument for the <c>requestDto</c> parameter doesn't exist or has already been
    /// deleted.
    public async Task UpdateAsync(int id, TUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        T entity = await GetRepository(_context)
            .Include(o => o.CreatedUser)
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Include(o => o.Photos)
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(entity),
                nameof(id),
                id.ToString());

        // Check if the current user has permission to edit this entity.
        if (!CanEdit(entity, _authorizationService))
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
            // Ensure the requesting user has permission to specify a new StatsDateTime.
            if (!CanSetStatsDateTime(_authorizationService))
            {
                throw new AuthorizationException();
            }

            // Prevent the entity's SupplyDateTime to be modified when the entity is locked.
            if (entity.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(typeof(T).Name)
                    .ReplacePropertyName(DisplayNames.StatsDateTime);
                throw new OperationException(
                    nameof(requestDto.StatsDateTime),
                    errorMessage);
            }

            // Assign the new StatsDateTime value only if it's different from the old one.
            if (requestDto.StatsDateTime.Value != entity.StatsDateTime)
            {
                // Validate the specified SupplyDateTime value from the request.
                try
                {
                    _statsService
                        .ValidateStatsDateTime(entity, requestDto.StatsDateTime.Value);
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
    /// <exception cref="ResourceNotFoundException">
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
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new ResourceNotFoundException(
                typeof(T).Name,
                nameof(id),
                id.ToString());

        // Check if the current user has permission to delete the entity.
        if (!CanDelete(entity, _authorizationService))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        DeleteItems(entity.Items, GetItemRepository, ProductEngagementType.Export);

        // Perform the deleting operation.
        try
        {
            await _context.SaveChangesAsync();
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

        // Order the results.
        switch (requestDto.OrderByField)
        {
            case nameof(OrderByFieldOptions.Amount):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(T.AmountAfterVatExpression).ThenBy(t => t.StatsDateTime)
                    : query.OrderByDescending(T.AmountAfterVatExpression)
                        .ThenByDescending(t => t.StatsDateTime);
                break;
            case nameof(OrderByFieldOptions.StatsDateTime):
                query = requestDto.OrderByAscending
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
    /// An instance of the <see cref="T"/> entity with the specified id.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the entity with the specified id doesn't exist or has already been deleted.
    /// </exception>
    protected async Task<T> GetEntityAsync(int id)
    {
        // Initialize query.
        IQueryable<T> query = GetRepository(_context)
            .Include(e => e.Customer)
            .Include(e => e.Items)
            .Include(e => e.Photos);

        return await base.GetEntityAsync(query, id);
    }

    /// <inheritdoc />
    protected override async Task<T> GetEntityAsync(IQueryable<T> query, int id)
    {
        return await base.GetEntityAsync(query, id);
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
    protected virtual void AssignExstingEntityProperty(T entity, TUpsertRequestDto requestDto)
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
            (TItem item, TItemRequestDto itemRequestDto) =>
            {
                item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit;
            });
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
            (TItem item, TItemRequestDto itemRequestDto) =>
            {
                item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit;
            },
            (TItem item, TItemRequestDto itemRequestDto) =>
            {
                item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit;
            });
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
    /// <exception cref="ResourceNotFoundException">
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
                throw new ResourceNotFoundException(
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
    /// Increments the statistics amounts with the <see cref="T"/> entity's amounts.
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
            IStatsInternalService<T, TUpdateHistory> statsService,
            bool isIncrementing);

    /// <summary>
    /// Determines whether the current user has enough permissions to set a value for the
    /// <c>StatsDateTime</c> property in the entity, used in the creating or updating
    /// operation.
    /// </summary>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanSetStatsDateTime(IAuthorizationInternalService service);

    /// <summary>
    /// Determines whether the current user has enough permissions to edit the specified
    /// entity, used in the creating or updating operation.
    /// </summary>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanEdit(T entity, IAuthorizationInternalService service);

    /// <summary>
    /// Determines whether the current user has enough permissions to delete a specific entity,
    /// used in the deleting operation.
    /// </summary>
    /// <param name="service">
    /// The service providing the authorization information.
    /// </param>
    /// <returns>
    /// A <see cref="bool"/> value representing the permission.
    /// </returns>
    protected abstract bool CanDelete(T entity, IAuthorizationInternalService service);
}
