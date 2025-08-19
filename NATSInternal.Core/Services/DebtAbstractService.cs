namespace NATSInternal.Core.Services;

/// <summary>
/// An abstract service to handle both debt incurrence and debt payment related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the debt entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TListRequestDto">
/// The type of the request DTO used in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TUpsertRequestDto">
/// The type of the request DTO used in the creating or updating operation.
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
/// <typeparam name="TUpdateHistoryResponseDto">
/// The type of the update history response dto, containing the data of the updating history
/// entity associated to the <typeparamref name="T"/> entity.
/// </typeparam>
/// <typeparam name="TUpdateHistoryDataDto">
/// The type of the update history data DTO, containing the data of a specific
/// <typeparamref name="T"/> entity instance after each modification, used in the updating
/// operation.
/// </typeparam>
/// <typeparam name="TCreatingAuthorizationResponseDto">
/// The type of the response DTO, containing the authorization information for the
/// <typeparamref name="TListResponseDto"/> DTO in the list retrieving operation.
/// </typeparam>
/// <typeparam name="TExistingAuthorizationResponseDto">
/// The type of the response DTO, containing the authorization information for the
/// <typeparamref name="TBasicResponseDto"/> and <see cref="TDetailResponseDto"/> DTOs, used in
/// the list retrieving and detail retrieving operations.
/// </typeparam>
internal abstract class DebtAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TUpsertRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TUpdateHistoryResponseDto,
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
    where T : class, IDebtEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto :
        IHasStatsListRequestDto,
        IHasCustomerListRequestDto
    where TUpsertRequestDto : IDebtUpsertRequestDto
    where TListResponseDto :
        IHasStatsResponseDto<
            TBasicResponseDto,
            TExistingAuthorizationResponseDto>,
        new()
    where TBasicResponseDto :
        class,
        IHasCustomerBasicResponseDto<TExistingAuthorizationResponseDto>
    where TDetailResponseDto : IDebtDetailResponseDto<
        TUpdateHistoryResponseDto,
        TExistingAuthorizationResponseDto>
    where TUpdateHistoryResponseDto : IDebtUpdateHistoryResponseDto
    where TCreatingAuthorizationResponseDto : class, IHasStatsCreatingAuthorizationResponseDto, new()
    where TExistingAuthorizationResponseDto : IHasStatsExistingAuthorizationResponseDto, new()
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService _statsService;

    protected DebtAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService statsService)
        : base(context, authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
        _statsService = statsService;
    }

    /// <inheritdoc />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
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
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Single(i => i.Name == nameof(OrderByFieldOption.StatsDateTime))
                .Name,
            DefaultAscending = false
        };
    }

    /// <summary>
    /// Retrieves a list of entities and month-year options, based on the specified filtering
    /// and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing conditions for the results.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> which representing the asynchronous operation, which result is
    /// a DTO, containing a list of entities and the additional information for pagination.
    /// </returns>
    protected async Task<EntityListDto<T>> GetListOfEntitiesAsync(TListRequestDto requestDto)
    {
        IQueryable<T> query = GetRepository(_context)
            .Include(e => e.Customer);


        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByFieldName
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.Amount):
                query = sortingByAscending
                    ? query.OrderBy(dp => dp.Amount).ThenBy(dp => dp.StatsDateTime)
                    : query.OrderByDescending(dp => dp.Amount)
                        .ThenByDescending(dp => dp.StatsDateTime);
                break;
            case nameof(OrderByFieldOption.StatsDateTime):
                query = sortingByAscending
                    ? query.OrderBy(dp => dp.StatsDateTime).ThenBy(dp => dp.Amount)
                    : query.OrderByDescending(dp => dp.StatsDateTime)
                        .ThenByDescending(dp => dp.Amount);
                break;
            default:
                throw new NotImplementedException();
        }

        return await GetListOfEntitiesAsync(query, requestDto);
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
        IQueryable<T> query = GetRepository(_context)
            .Include(d => d.Customer)
            .Include(d => d.CreatedUser).ThenInclude(u => u.Roles);

        return await base.GetEntityAsync(query, id);
    }

    /// <summary>
    /// Creates a new debt entity based on the specified data from the request.
    /// </summary>
    /// <param name="requestDto">
    /// An DTO containing the data for the creating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asychronous operation, which result is the id of
    /// the new debt entity.
    /// </returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to specify a value
    /// for the <c>StatsDateTime</c> property in the <paramref name="requestDto"/> argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when the information of the requesting user has already been deleted before the
    /// operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The customer specified by the <c>CustomerId</c> in the <paramref name="requestDto"/>
    /// argument doesn't exist or has already been deleted.
    /// - The remaining debt amount of the specified customer becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Throws the <c>_debtType</c> field has a value which handling logic hasn't been
    /// implemented yet.
    /// </exception>
    public virtual async Task<int> CreateAsync(TUpsertRequestDto requestDto)
    {
        // Fetch the customer entity with the specified id.
        Customer customer = await _context.Customers.FindAsync(requestDto.CustomerId)
            ?? throw new OperationException(ErrorMessages.NotFound);

        // Determining the stats datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenCreating())
            {
                throw new AuthorizationException();
            }

            statsDateTime = requestDto.StatsDateTime.Value;
        }
        
        // Initialize the entity.
        T entity = new T
        {
            Amount = requestDto.Amount,
            Note = requestDto.Note,
            StatsDateTime = statsDateTime,
            CustomerId = requestDto.CustomerId,
            CreatedUserId = _authorizationService.GetUserId()
        };

        GetRepository(_context).Add(entity);

        // Update the cached debt amount.
        AdjustCustomerCachedDebtAmount(customer, requestDto.Amount);
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();

            // Increment the stats.
            await AdjustStatsAsync(entity, _statsService, true);
            
            // Commit the transaction, finish all operations.
            await transaction.CommitAsync();
            
            return entity.Id;
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            HandleCreateOrUpdateException(sqlException);
            throw;
        }
    }
    
    /// <summary>
    /// Updates an existing debt entity, based on its id and the provided data.
    /// </summary>
    /// <param name="id">
    /// The id of the debt payment to update.
    /// </param>
    /// <param name="requestDto">
    /// A DTO, containing the data for the updating operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the debt payment specified by the <paramref name="id"/> argument doesn't
    /// exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws under the following circumstances:<br/>
    /// - When the requesting user doesn't have enough permissions to update the debt payment.
    /// - When the requesting user can update the debt payment, but doesn't have enough
    /// permissions to specify a value for the <c>StatsDateTime</c> property in the
    /// <paramref name="requestDto"/>.
    /// </exception>
    /// <exception cref="ValidationException">
    /// Throws when the value of the <c>StatsDateTime</c> property in the <c>requestDto</c>
    /// argument is invalid.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the <c>StatsDateTime</c> property in the <paramref name="requestDto"/> is
    /// specified a value while the debt payment has already been locked.
    /// - When the remaining debt amount of the associated customer becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:<br/>
    /// - When the debt payment has been modified by another process before the operation.<br/>
    /// - When the information of the requesting user has been deleted by another process
    /// before the operation.
    /// </exception>
    public virtual async Task UpdateAsync(int id, TUpsertRequestDto requestDto)
    {
        // Prepare the query to fetch the entity.
        IQueryable<T> query = GetRepository(_context)
            .Include(e => e.Customer).ThenInclude(c => c.DebtIncurrences)
            .Include(e => e.Customer).ThenInclude(c => c.DebtPayments)
            .Include(d => d.CreatedUser);

        // Filter by id and soft deleted.
        query = query.Where(dp => dp.Id == id && !dp.IsDeleted);

        // Fetch the entity.
        T entity = await query.SingleOrDefaultAsync()
            ?? throw new NotFoundException();
        
        // Check if the current user has permission to edit the debt payment.
        if (!CanEdit(entity))
        {
            throw new AuthorizationException();
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Decrement the old stats and store the old data for update history logging.
        await AdjustStatsAsync(entity, _statsService, false);
        TUpdateHistoryDataDto oldData = InitializeUpdateHistoryDataDto(entity);

        // Update the paid datetime if specified.
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
                // Verify if the amount has been changed, and with the new amount, the remaning
                // debt amount won't be negative.
                if (requestDto.Amount != entity.Amount)
                {
                    entity.Amount = requestDto.Amount;
                    if (entity.Customer.DebtAmount < 0)
                    {
                        throw new OperationException(
                            nameof(requestDto.Amount),
                            ErrorMessages.NegativeRemainingDebtAmount);
                    }
                }
                
                // Validate the specified StatsDateTime from the request.
                try
                {
                    _statsService.ValidateStatsDateTime<T, TUpdateHistory>(
                        entity,
                        requestDto.StatsDateTime.Value);
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.StatsDateTime);
                    throw new OperationException(
                        nameof(DisplayNames.StatsDateTime),
                        errorMessage);
                }

                // The specified StatsDateTime is valid, assign it to the entity.
                entity.StatsDateTime = requestDto.StatsDateTime.Value;
            }
        }

        // Verify that with the new paid amount, the customer's remaining debt amount will
        // not be negative.
        if (entity.Customer.DebtAmount < 0)
        {
            const string amountErrorMessage = ErrorMessages.NegativeRemainingDebtAmount;
            throw new OperationException(nameof(requestDto.Amount), amountErrorMessage);
        }

        // Update debt amount and adjust cached amount.
        if (entity.Amount != requestDto.Amount)
        {
            long differentAmount = requestDto.Amount - entity.Amount;
            AdjustCustomerCachedDebtAmount(entity.Customer, differentAmount);
            entity.Amount = requestDto.Amount;
        }
        
        // Update note.
        entity.Note = requestDto.Note;
        
        // Store new data for update history logging.
        TUpdateHistoryDataDto newData = InitializeUpdateHistoryDataDto(entity);
        
        // Log update history.
        LogUpdateHistory(entity, oldData, newData, requestDto.UpdatedReason);
        
        // Perform the update operations.
        try
        {
            await _context.SaveChangesAsync();
            
            // Increment the new stats.
            await AdjustStatsAsync(entity, _statsService, true);
            
            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Handling concurrecy exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
            
            // Handling data exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleCreateOrUpdateException(sqlException);
            }
            
            throw;
        }
    }

    /// <summary>
    /// Deletes an existing debt entity by its id.
    /// </summary>
    /// <param name="id">The ID of the debt entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the user doesn't have permission to delete the specifed debt entity.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Throws when the debt entity with the specified id doens't exist or has already been
    /// deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the debt entity's deletion is restricted due to the existence of some
    /// related data.
    /// </exception>
    public virtual async Task DeleteAsync(int id)
    {
        // Fetch and ensure the entity with the given debtPaymentId exists in the database.
        T entity = await GetRepository(_context)
            .Include(d => d.Customer).ThenInclude(c => c.DebtIncurrences)
            .Include(d => d.Customer).ThenInclude(c => c.DebtPayments)
            .Where(dp => dp.Id == id && !dp.IsDeleted)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
        
        // Ensure the user has permission to delete this debt entity.
        if (!CanDelete(entity))
        {
            throw new AuthorizationException();
        }

        // Verify that if this debt entity is closed.
        if (entity.IsLocked)
        {
            string errorMessage = ErrorMessages.ModificationTimeExpired
                .ReplaceResourceName(DisplayNames.Get(typeof(T).Name));
            throw new OperationException(errorMessage);
        }
        
        // Ensure the remaining debt amount of the customer isn't negative after the operation.
        if (entity.Customer.DebtAmount < entity.Amount)
        {
            throw new OperationException(ErrorMessages.NegativeRemainingDebtAmount);
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Perform deleting operation and adjust stats.
        try
        {
            GetRepository(_context).Remove(entity);
            await _context.SaveChangesAsync();
            
            // The entity has been deleted successfully, adjust the stats based on debt type.
            await AdjustStatsAsync(entity, _statsService, true);
            
            // Commit the transaction, finish the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
            
            // Handle deleting restricted exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                // Soft delete when the entity is restricted to be deleted.
                if (exceptionHandler.IsDeleteOrUpdateRestricted)
                {
                    entity.IsDeleted = true;
                    
                    // Adjust the stats.
                    await AdjustStatsAsync(entity, _statsService, false);

                    // Save changes and commit the transaction again, finish the operation.
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }

    /// <summary>
    /// Handles the exception thrown by the database during the creating or updating operation.
    /// </summary>
    /// <param name="exception">
    /// The exception thrown by the database during the operation.
    /// </param>
    /// <exception cref="OperationException">
    /// Throws when the <c>exception</c> indicates that the <c>CustomerId</c> foreign key
    /// references to a non-existent customer.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when the <c>exception</c> indicates that the information of the requesting user
    /// has been deleted before the operation.
    /// </exception>
    private static void HandleCreateOrUpdateException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            string violatedFieldName = exceptionHandler.ViolatedFieldName;
            // The foreign key CustomerId references to a non-existent customer entity.
            if (violatedFieldName == GetPropertyName<T>(e => e.CustomerId))
            {
                string errorMessage = ErrorMessages.NotFound
                    .ReplaceResourceName(DisplayNames.Customer);
                throw new OperationException("customerId", errorMessage);
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
    /// Adjusts the statistics amounts with the <typeparamref name="T"/> entity's amounts.
    /// </summary>
    /// <param name="entity">
    /// The instance of the entity with which the associated statistics is to be incremented.
    /// </param>
    /// <param name="service">
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
            IStatsInternalService service,
            bool isIncrementing);

    /// <summary>
    /// Adjusts the cached total debt amount (debt incurrences or debt payments) of the
    /// specified customer.
    /// </summary>
    /// <param name="customer">
    /// The customer entity with the cached debt amount to be adjusted.
    /// </param>
    /// <param name="differentAmount">
    /// The amount representing the difference between the old and the new debt amount to be
    /// adjusted.
    /// </param>
    protected abstract void AdjustCustomerCachedDebtAmount(
            Customer customer,
            long differentAmount);
}