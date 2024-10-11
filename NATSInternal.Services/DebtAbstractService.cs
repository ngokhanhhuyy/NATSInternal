namespace NATSInternal.Services;

/// <summary>
/// An abstract service to handle both debt incurrence and debt payment related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the debt entity.
/// </typeparam>
/// <typeparam name="TUpdateHistory">
/// The type of the update history entity associated to the <see cref="T"/> entity.
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
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    : FinancialEngageableAbstractService<
        T,
        TUpdateHistory,
        TListRequestDto,
        TListResponseDto,
        TBasicResponseDto,
        TDetailResponseDto,
        TUpdateHistoryResponseDto,
        TUpdateHistoryDataDto,
        TListAuthorizationResponseDto,
        TAuthorizationResponseDto>
    where T : class, IDebtEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
    where TListRequestDto :
        IFinancialEngageableListRequestDto,
        ICustomerEngageableListRequestDto
    where TUpsertRequestDto : IDebtUpsertRequestDto
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
    where TUpdateHistoryResponseDto : IUpdateHistoryResponseDto
    where TListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
    where TAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService<T, TUpdateHistory> _statsService;

    protected DebtAbstractService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<T, TUpdateHistory> statsService,
            IMonthYearService<T, TUpdateHistory> monthYearService)
        : base(context, authorizationService, monthYearService)
    {
        _context = context;
        _authorizationService = authorizationService;
        _statsService = statsService;
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
    /// Retrieves the details of a specific debt entity by its id.
    /// </summary>
    /// <param name="id">
    /// The id of the debt entity to retrieve.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a DTO
    /// containing the details of the debt payment.
    /// </returns>
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt entity with the specified <c>id</c> arguments doesn't exist or
    /// has already been deleted.
    /// </exception>
    public virtual async Task<TDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<T> query = GetRepository(_context)
            .Where(d => d.Id == id)
            .Where(d => !d.IsDeleted);

        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = CanAccessUpdateHistories(_authorizationService);
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(d => d.UpdateHistories);
        }

        query = query
            .Include(d => d.Customer)
            .Include(d => d.CreatedUser).ThenInclude(u => u.Roles);

        // Fetch the entity with the given id and ensure it exists in the database.
        T entity = await query
            .AsSplitQuery()
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();

        return await base.GetDetailAsync(query);
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
    /// for the <c>StatsDateTime</c> property in the <c>requestDto</c> argument.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when the information of the requesting user has already been deleted before the
    /// operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - The customer specified by the <c>CustomerId</c> in the <c>requestDto</c> argument
    /// doesn't exist or has already been deleted.
    /// - The remaining debt amount of the specified customer becomes negative after the
    /// operation.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Throws the <c>_debtType</c> field has a value which handling logic hasn't been
    /// implemented yet.
    /// </exception>
    public virtual async Task<int> CreateAsync(TUpsertRequestDto requestDto)
    {
        // Determining the stats datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the current user has permission to specify a value for StatsDateTime.
            if (CanSetStatsDateTime(_authorizationService))
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

        CustomizeEntityInitialization(entity, requestDto);

        GetRepository(_context).Add(entity);
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();

            // The entity is saved successfully, adjust the stats based on the debt type.
            DateOnly statsDate = DateOnly.FromDateTime(entity.StatsDateTime);
            await IncrementStatsAsync(entity.Amount, statsDate, _statsService);
            
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
    /// <exception cref="ResourceNotFoundException">
    /// Throws when the debt payment specified by the <c>id</c> argument doesn't exist or has
    /// already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws under the following circumstances:<br/>
    /// - When the requesting user doesn't have enough permissions to update the debt payment.
    /// - When the requesting user can update the debt payment, but doesn't have enough
    /// permissions to specify a value for the <c>SupplyDateTime</c> property in the
    /// <c>requestDto</c> argument.
    /// </exception>
    /// <exception cref="ValidationException">
    /// Throws when the value of the <c>SupplyDateTime</c> property in the <c>requestDto</c>
    /// argument is invalid.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws under the following circumstances:<br/>
    /// - When the <c>SupplyDateTime</c> property in the <c>requestDto</c> argument is
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
    /// <exception cref="NotImplementedException">
    /// Throws the <c>_debtType</c> field has a value which handling logic hasn't been
    /// implemented yet.
    /// </exception>
    public virtual async Task UpdateAsync(
            int id,
            TUpsertRequestDto requestDto)
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
            ?? throw new ResourceNotFoundException();
        
        // Check if the current user has permission to edit the debt payment.
        if (!InitializeAuthorizationResponseDto(_authorizationService, entity).CanEdit)
        {
            throw new AuthorizationException();
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Adjust the stats and store the old data for update history logging.
        DateOnly oldDate = DateOnly.FromDateTime(entity.StatsDateTime);
        await IncrementStatsAsync(-entity.Amount, oldDate, _statsService);
        TUpdateHistoryDataDto oldData = InitializeUpdateHistoryDataDto(entity);

        // Update the paid datetime if specified.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Check if the current user has permission to change the created datetime.
            if (!_authorizationService.CanSetDebtIncurrenceIncurredDateTime())
            {
                throw new AuthorizationException();
            }

            // Prevent StatsDateTime to be modified when the entity is already locked.
            if (entity.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.Get(typeof(T).Name))
                    .ReplacePropertyName(DisplayNames.Get(entity.GetStatsPropertyName()));
                throw new OperationException(
                    entity.GetStatsPropertyName(),
                    errorMessage);
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
                
                // Validate the specified SupplyDateTime from the request.
                try
                {
                    _statsService.ValidateStatsDateTime(
                        entity,
                        requestDto.StatsDateTime.Value);
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.Get(entity.GetStatsPropertyName()));
                    throw new OperationException(
                        entity.GetStatsPropertyName(),
                        errorMessage);
                }

                // The specified StatsDateTime is valid, assign it to the entity.
                entity.StatsDateTime = requestDto.StatsDateTime.Value;
            }
        }

        // Verify that with the new paid amount, the customer's remaining debt amount will
        // not be negative.
        if (entity.Customer.DebtAmount < requestDto.Amount)
        {
            const string amountErrorMessage = ErrorMessages.NegativeRemainingDebtAmount;
            throw new OperationException(nameof(requestDto.Amount), amountErrorMessage);
        }
        
        // Update other properties.
        entity.Amount = requestDto.Amount;
        entity.Note = requestDto.Note;
        
        // Store new data for update history logging.
        TUpdateHistoryDataDto newData = InitializeUpdateHistoryDataDto(entity);
        
        // Log update history.
        LogUpdateHistory(entity, oldData, newData, requestDto.UpdatedReason);
        
        // Perform the update operations.
        try
        {
            await _context.SaveChangesAsync();
            
            // The debt payment can be updated successfully without any error, djust the stats.
            DateOnly newDate = DateOnly.FromDateTime(entity.StatsDateTime);
            await IncrementStatsAsync(-entity.Amount, newDate, _statsService);
            
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
    /// <exception cref="ResourceNotFoundException">
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
            ?? throw new ResourceNotFoundException();
        
        // Ensure the user has permission to delete this debt entity.
        if (!CanDelete(_authorizationService))
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
            DateOnly statsDate = DateOnly.FromDateTime(entity.StatsDateTime);
            await IncrementStatsAsync(-entity.Amount, statsDate, _statsService);
            
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
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                // Soft delete when the entity is restricted to be deleted.
                if (exceptionHandler.IsDeleteOrUpdateRestricted)
                {
                    entity.IsDeleted = true;
                    
                    // Adjust the stats.
                    DateOnly createdDate = DateOnly.FromDateTime(entity.StatsDateTime);
                    await _statsService
                        .IncrementDebtIncurredAmountAsync(entity.Amount, createdDate);
                    
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
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
        exceptionHandler.Handle(exception);
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            switch (exceptionHandler.ViolatedFieldName)
            {
                // The foreign key CustomerId references to a non-existent customer entity.
                case "customer_id":
                    string errorMessage = ErrorMessages.NotFound
                        .ReplaceResourceName(DisplayNames.Customer);
                    throw new OperationException("customerId", errorMessage);
                
                // The foreign key CreatedUserId references to a user which might have been
                // deleted.
                default:
                    throw new ConcurrencyException();
            }
        }
    }

    /// <summary>
    /// Customizes the data assignment from the request DTO to the new initialized entity.
    /// </summary>
    /// <remarks>
    /// This method is called immediately right after the new entity has been initialized.
    /// Override this method to provide the initialization customization.
    /// </remarks>
    /// <param name="entity">
    /// The entity of which properties data assignment are to be customized.
    /// </param>
    /// <param name="requestDto">
    /// A DTO containing the data for the new entity creating operation.
    /// </param>
    protected virtual void CustomizeEntityInitialization(
            T entity,
            TUpsertRequestDto requestDto)
    {
    }
    
    /// <summary>
    /// Increment the statistics amount based on the specified <see cref="T"/> entity.
    /// </summary>
    /// <param name="amount">
    /// The amount of the entity to increment.
    /// </param>
    /// <param name="date">
    /// The statistics date of the entity to increment.
    /// <param name="service">
    /// An instance of the service to handle the statistics amount incrementing operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    protected abstract Task IncrementStatsAsync(
            long amount,
            DateOnly date,
            IStatsInternalService<T, TUpdateHistory> service);
}