namespace NATSInternal.Services;

/// <inheritdoc cref="IDebtPaymentService" />
internal class DebtPaymentService
    :
        DebtService<
            DebtPayment,
            DebtPaymentUpdateHistory,
            DebtPaymentListRequestDto,
            DebtPaymentUpsertRequestDto,
            DebtPaymentListResponseDto,
            DebtPaymentBasicResponseDto,
            DebtPaymentDetailResponseDto,
            DebtPaymentUpdateHistoryResponseDto,
            DebtPaymentListAuthorizationResponseDto,
            DebtPaymentAuthorizationResponseDto>,
        IDebtPaymentService
{
    private readonly IUpdateHistoryService<DebtPayment, DebtPaymentUpdateHistory, DebtPaymentUpdateHistoryDataDto> _updateHistoryService;

    public DebtPaymentService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<DebtPayment, DebtPaymentUpdateHistory> statsService,
            IUpdateHistoryService<DebtPayment, DebtPaymentUpdateHistory, DebtPaymentUpdateHistoryDataDto> updateHistoryService,
            IMonthYearService<DebtPayment, DebtPaymentUpdateHistory> monthYearService)
        : base(
            context,
            authorizationService,
            statsService,
            monthYearService,
            DebtType.DebtPayment)
    {
        _updateHistoryService = updateHistoryService;
    }

    protected override DbSet<DebtPayment> GetRepository(DatabaseContext context)
    {
        return context.DebtPayments;
    }

    protected override IOrderedQueryable<DebtPayment> SortListQuery(
            IQueryable<DebtPayment> query,
            DebtPaymentListRequestDto requestDto)
    {
        switch (requestDto.OrderByField)
        {
            case nameof(DebtIncurrenceListRequestDto.FieldOptions.Amount):
                return requestDto.OrderByAscending
                    ? query.OrderBy(dp => dp.Amount).ThenBy(dp => dp.PaidDateTime)
                    : query.OrderByDescending(dp => dp.Amount)
                        .ThenByDescending(dp => dp.PaidDateTime);
            default:
                return requestDto.OrderByAscending
                    ? query.OrderBy(dp => dp.PaidDateTime).ThenBy(dp => dp.Amount)
                    : query.OrderByDescending(dp => dp.PaidDateTime)
                        .ThenByDescending(dp => dp.Amount);
        }
    }

    protected override IQueryable<DebtPayment> FilterByMonthYearListQuery(
            IQueryable<DebtPayment> query,
            DateTime startingDateTime,
            DateTime endingDateTime)
    {
        return query
            .Where(dp => dp.PaidDateTime >= startingDateTime)
            .Where(dp => dp.PaidDateTime < endingDateTime);
    }
    
    protected override DebtPaymentBasicResponseDto InitializeBasicResponseDto(
            DebtPayment debtPayment)
    {
        return new DebtPaymentBasicResponseDto(
            debtPayment,
            _authorizationService.GetDebtPaymentAuthorization(debtPayment));
    }

    protected override DebtPaymentDetailResponseDto InitializeDetailResponseDto(
            DebtPayment debtPayment,
            bool shouldIncludeUpdateHistories)
    {
        return new DebtPaymentDetailResponseDto(
            debtPayment,
            _authorizationService.GetDebtPaymentAuthorization(debtPayment),
            mapUpdateHistories: shouldIncludeUpdateHistories);
    }

    /// <inheritdoc />
    public async Task<DebtPaymentDetailResponseDto> GetDetailAsync(int id)
    {
        return await base.GetDetailAsync(
            id,
            (debtPayment, shouldIncludeUpdateHistories) => new DebtPaymentDetailResponseDto(
                debtPayment,
                _authorizationService.GetDebtPaymentAuthorization(debtPayment),
                mapUpdateHistories: shouldIncludeUpdateHistories));
    }
    
    /// <inheritdoc />
    public async Task UpdateAsync(int id, DebtPaymentUpsertRequestDto requestDto)
    {
        // Fetch and ensure the entity with the given debtPaymentId exists in the database.
        DebtPayment debtPayment = await _context.DebtPayments
            .Include(d => d.Customer).ThenInclude(c => c.DebtIncurrences)
            .Include(d => d.CreatedUser)
            .Where(dp => dp.Id == id && !dp.IsDeleted)
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();
        
        // Check if the current user has permission to edit the debt payment.
        if (!_authorizationService.CanEditDebtPayment(debtPayment))
        {
            throw new AuthorizationException();
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Store the old data and create new data for stats adjustment.
        long oldAmount = debtPayment.Amount;
        DateOnly oldPaidDate = DateOnly.FromDateTime(debtPayment.PaidDateTime);
        DebtPaymentUpdateHistoryDataDto oldData;
        oldData = new DebtPaymentUpdateHistoryDataDto(debtPayment);

        // Update the paid datetime if specified.
        if (requestDto.PaidDateTime.HasValue)
        {
            // Check if the current user has permission to change the created datetime.
            if (!_authorizationService.CanSetDebtIncurredDateTime())
            {
                throw new AuthorizationException();
            }

            // Prevent SupplyDateTime to be modified when the debt payment is already locked.
            if (debtPayment.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.DebtPayment)
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(
                    nameof(requestDto.PaidDateTime),
                    errorMessage);
            }
            
            // Assign the new SupplyDateTime value only if it's different from the old one.
            if (requestDto.PaidDateTime.Value != debtPayment.PaidDateTime)
            {
                // Verify if the amount has been changed, and with the new amount, the remaning
                // debt amount won't be negative.
                if (requestDto.Amount != debtPayment.Amount)
                {
                    long amountDifference = requestDto.Amount - debtPayment.Amount;
                    if (debtPayment.Customer.DebtAmount + amountDifference < 0)
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
                        debtPayment,
                        requestDto.PaidDateTime.Value);
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.PaidDateTime);
                    throw new OperationException(
                        nameof(requestDto.PaidDateTime),
                        errorMessage);
                }

                // The specified SupplyDateTime is valid, assign it to the debt payment.
                debtPayment.PaidDateTime = requestDto.PaidDateTime.Value;
            }
        }

        // Verify that with the new paid amount, the customer's remaining debt amount will
        // not be negative.
        if (debtPayment.Customer.DebtAmount < requestDto.Amount)
        {
            const string amountErrorMessage = ErrorMessages.NegativeRemainingDebtAmount;
            throw new OperationException(nameof(requestDto.Amount), amountErrorMessage);
        }
        
        // Update other properties.
        debtPayment.Amount = requestDto.Amount;
        debtPayment.Note = requestDto.Note;
        
        // Store new data for update history logging.
        DebtPaymentUpdateHistoryDataDto newData;
        newData = new DebtPaymentUpdateHistoryDataDto(debtPayment);
        
        // Log update history.
        _updateHistoryService
            .LogUpdateHistory(debtPayment, oldData, newData, requestDto.UpdatingReason);
        
        // Perform the update operations.
        try
        {
            await _context.SaveChangesAsync();
            
            // The debt payment can be updated successfully without any error.
            // Adjust the stats.
            // Revert the old stats.
            await _statsService.IncrementDebtAmountAsync(-oldAmount, oldPaidDate);
            
            // Add new stats.
            DateOnly newPaidDate = DateOnly.FromDateTime(debtPayment.PaidDateTime);
            await _statsService.IncrementDebtAmountAsync(debtPayment.Amount, newPaidDate);
            
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
    
    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch and ensure the entity with the given debtPaymentId exists in the database.
        DebtPayment debtPayment = await _context.DebtPayments
            .Include(d => d.Customer).ThenInclude(c => c.DebtPayments)
            .Where(dp => dp.Id == id && !dp.IsDeleted)
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException();
        
        // Ensure the user has permission to delete this debt payment.
        if (!_authorizationService.CanDeleteDebtPayment())
        {
            throw new AuthorizationException();
        }

        // Verify that if this debt payment is closed.
        if (debtPayment.IsLocked)
        {
            string errorMessage = ErrorMessages.ModificationTimeExpired
                .ReplaceResourceName(DisplayNames.DebtPayment);
            throw new OperationException(errorMessage);
        }
        
        // Verify that if this debt payment is deleted, will the remaining debt amount be
        // negative.
        if (debtPayment.Customer.DebtAmount < debtPayment.Amount)
        {
            throw new OperationException(ErrorMessages.NegativeRemainingDebtAmount);
        }
        
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();
        
        // Perform deleting operation and adjust stats.
        try
        {
            _context.DebtPayments.Remove(debtPayment);
            await _context.SaveChangesAsync();
            
            // DebtIncurrence payment has been deleted successfully, adjust the stats.
            DateOnly createdDate = DateOnly.FromDateTime(debtPayment.PaidDateTime);
            await _statsService
                .IncrementDebtPaidAmountAsync(- debtPayment.Amount, createdDate);
            
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
                    debtPayment.IsDeleted = true;
                    
                    // Adjust the stats.
                    DateOnly createdDate = DateOnly.FromDateTime(debtPayment.PaidDateTime);
                    await _statsService
                        .IncrementDebtAmountAsync(debtPayment.Amount, createdDate);
                    
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
    /// An instance of the <see cref="MySqlException"/> class, containing the details of the
    /// error.
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
}