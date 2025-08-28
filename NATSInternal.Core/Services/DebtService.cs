namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class DebtService : IDebtService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly ISummaryInternalService _summaryService;
    private readonly IUpdateHistoryService<Debt, DebtUpdateHistoryData> _updateHistoryService;
    private readonly IListQueryService _listQueryService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<DebtListRequestDto> _listValidator;
    private readonly IValidator<DebtUpsertRequestDto> _upsertValidator;
    #endregion

    #region Constructors
    public DebtService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            ISummaryInternalService summaryService,
            IUpdateHistoryService<Debt, DebtUpdateHistoryData> updateHistoryService,
            IListQueryService listQueryService,
            IDbExceptionHandler exceptionHandler,
            IValidator<DebtListRequestDto> listValidator,
            IValidator<DebtUpsertRequestDto> upsertValidator)
    {
        _context = context;
        _authorizationService = authorizationService;
        _summaryService = summaryService;
        _updateHistoryService = updateHistoryService;
        _exceptionHandler = exceptionHandler;
        _listQueryService = listQueryService;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<DebtListResponseDto> GetListAsync(
            DebtListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);

        // Prepare the query.
        IQueryable<Debt> query = _context.Debts.Include(d => d.Customer);

        // Determine the field and the direction the sort.
        string sortByFieldName = requestDto.SortByFieldName ?? GetListSortingOptions().DefaultFieldName;
        bool sortByAscending = requestDto.SortByAscending ?? GetListSortingOptions().DefaultAscending;
        switch (sortByFieldName)
        {
            case nameof(DebtListRequestDto.FieldToSort.Amount):
                query = query
                    .ApplySorting(d => d.Amount, sortByAscending)
                    .ApplySorting(d => d.StatsDateTime, sortByAscending);
                break;
            case nameof(DebtListRequestDto.FieldToSort.StatsDateTime):
                query = query
                    .ApplySorting(d => d.StatsDateTime, sortByAscending)
                    .ApplySorting(d => d.Amount, sortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        // Filter by type.
        if (requestDto.Type.HasValue)
        {
            query = query.Where(d => d.Type == requestDto.Type);
        }

        // Filter by customer id.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(d => d.CustomerId == requestDto.CustomerId.Value);
        }

        // Filter by created user id.
        if (requestDto.CreatedUserId.HasValue)
        {
            query = query.Where(d => d.CreatedUserId == requestDto.CreatedUserId.Value);
        }

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (debt) =>
            {
                DebtExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
                    .GetExistingAuthorization<Debt, DebtUpdateHistoryData, DebtExistingAuthorizationResponseDto>(debt);

                return new DebtBasicResponseDto(debt, authorizationResponseDto);
            },
            (basicResponseDtos, pageCount) => new DebtListResponseDto(basicResponseDtos, pageCount),
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<DebtDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Debt debt = await _context.Debts
            .Include(d => d.Customer)
            .Include(d => d.CreatedUser).ThenInclude(u => u.Roles)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        DebtExistingAuthorizationResponseDto authorization = _authorizationService
            .GetExistingAuthorization<Debt, DebtUpdateHistoryData, DebtExistingAuthorizationResponseDto>(debt);

        return new DebtDetailResponseDto(debt, authorization);
    }

    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions = new()
        {
            new()
            {
                Name = nameof(OrderByFieldOption.Amount),
                DisplayName = DisplayNames.Amount
            },
            new()
            {
                Name = nameof(OrderByFieldOption.StatsDateTime),
                DisplayName = DisplayNames.StatsDateTime
            }
        };

        return new()
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Where(d => d.Name == nameof(DebtListRequestDto.FieldToSort.StatsDateTime))
                .Select(d => d.Name)
                .Single(),
            DefaultAscending = false
        };
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(DebtUpsertRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet();
        });

        // Fetch the customer entity with the specified id.
        Customer customer = await _context.Customers
            .SingleOrDefaultAsync(c => c.Id == requestDto.CustomerId, cancellationToken)
            ?? throw OperationException.NotFound(
                new object[] { nameof(requestDto.CustomerId) },
                DisplayNames.Customer
            );

        // Determining the stats datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!_authorizationService.CanSetStatsDateTimeWhenCreating<Debt, DebtUpdateHistoryData>())
            {
                throw new AuthorizationException();
            }

            statsDateTime = requestDto.StatsDateTime.Value;
        }

        // Initialize the debt entity.
        Debt debt = new()
        {
            Type = requestDto.Type,
            Amount = requestDto.Amount,
            Note = requestDto.Note,
            StatsDateTime = statsDateTime,
            CustomerId = requestDto.CustomerId,
            CreatedUserId = _authorizationService.GetUserId()
        };

        _context.Debts.Add(debt);

        // Adjust the cached debt amount.
        AdjustCustomerRemainingDebtAmount(debt, debt.Amount);

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            // Increment the stats.
            DateOnly statsDate = DateOnly.FromDateTime(debt.StatsDateTime);
            if (debt.Type == DebtType.Incurrence)
            {
                await _summaryService.IncrementDebtIncurredAmountAsync(debt.Amount, statsDate, cancellationToken);
            }
            else
            {
                await _summaryService.IncrementDebtPaidAmountAsync(debt.Amount, statsDate, cancellationToken);
            }


            // Commit the transaction, finish all operations.
            await transaction.CommitAsync(cancellationToken);

            return debt.Id;
        }
        catch (DbUpdateException exception)
        {
            CoreException? convertedException = TryHandleDbUpdateException(exception);
            if (convertedException is null)
            {
                throw;
            }

            throw convertedException;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(
            Guid id,
            DebtUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet();
        });

        // Fetch the debt entity and ensure it exists in the database.
        Debt debt = await _context.Debts
            .Include(e => e.Customer)
            .Include(d => d.CreatedUser)
            .SingleOrDefaultAsync(d => d.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        // Check if the current user has permission to edit the debt.
        if (!_authorizationService.CanEdit<Debt, DebtUpdateHistoryData>(debt))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        // Decrement the old summary value and store the old data for update history creation.
        await AdjustSummaryAsync(debt, -debt.Amount, cancellationToken);

        DebtUpdateHistoryData oldData = new(debt);

        // Update the paid datetime if specified.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!_authorizationService.CanSetStatsDateTimeWhenEditing<Debt, DebtUpdateHistoryData>(debt))
            {
                throw new AuthorizationException();
            }

            // Assign the new StatsDateTime value only if it's different from the old one.
            if (requestDto.StatsDateTime.Value != debt.StatsDateTime)
            {
                // Verify if the amount has been changed, and with the new amount, the remaning debt amount won't be
                // negative.
                if (requestDto.Amount != debt.Amount)
                {
                    debt.Amount = requestDto.Amount;
                    if (debt.Customer.RemainingDebtAmount < 0)
                    {
                        throw new OperationException(
                            new object[] { nameof(requestDto.Amount) },
                            ErrorMessages.NegativeRemainingDebtAmount
                        );
                    }
                }

                // Validate the specified StatsDateTime from the request.
                bool isStatsDateTimeValid = _summaryService.IsStatsDateTimeValid<Debt, DebtUpdateHistoryData>(
                    debt,
                    requestDto.StatsDateTime.Value,
                    out string statsDateTimeErrorMessage);
                
                if (!isStatsDateTimeValid)
                {
                    throw new OperationException(
                        new object[] { nameof(requestDto.StatsDateTime) },
                        statsDateTimeErrorMessage
                    );
                }

                // The specified StatsDateTime is valid, assign it to the entity.
                debt.StatsDateTime = requestDto.StatsDateTime.Value;
            }
        }

        // Verify that with the new paid amount, the customer's remaining debt amount will not be negative.
        if (debt.Customer.RemainingDebtAmount < 0)
        {
            throw new OperationException(
                new object[] { nameof(requestDto.Amount) },
                ErrorMessages.NegativeRemainingDebtAmount
            );
        }

        // Update debt amount and adjust cached amount.
        if (debt.Amount != requestDto.Amount)
        {
            long differentAmount = requestDto.Amount - debt.Amount;
            AdjustCustomerRemainingDebtAmount(debt, differentAmount);
            debt.Amount = requestDto.Amount;
        }

        // Update note.
        debt.Note = requestDto.Note;

        // Store new data for update history logging.
        DebtUpdateHistoryData newData = new(debt);

        // Log update history.
        _updateHistoryService.AddUpdateHistory(debt, oldData, newData, requestDto.UpdatedReason);

        // Perform the update operations.
        try
        {
            await _context.SaveChangesAsync(cancellationToken);

            // Increment the new summary value.
            await AdjustSummaryAsync(debt, debt.Amount, cancellationToken);

            // Commit the transaction and finish the operation.
            await transaction.CommitAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            CoreException? convertedException = TryHandleDbUpdateException(exception);
            if (convertedException is null)
            {
                throw;
            }

            throw convertedException;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Fetch and ensure the debt entity with the given id exists in the database.
        Debt debt = await _context.Debts
            .Include(d => d.Customer).ThenInclude(c => c.Debts)
            .Where(dp => dp.Id == id && !dp.IsDeleted)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException();

        // Ensure the user has permission to delete this debt entity.
        if (!_authorizationService.CanDelete<Debt, DebtUpdateHistoryData>(debt))
        {
            throw new AuthorizationException();
        }

        // Verify that if this debt entity is locked.
        if (debt.IsLocked())
        {
            string errorMessage = ErrorMessages.ModificationTimeExpired.ReplaceResourceName(DisplayNames.Debt);
            throw new OperationException(errorMessage);
        }

        // Ensure the remaining debt amount of the customer isn't negative after the operation.
        if (debt.Customer.RemainingDebtAmount < debt.Amount)
        {
            throw new OperationException(ErrorMessages.NegativeRemainingDebtAmount);
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        // Perform deleting operation and adjust stats.
        try
        {
            _context.Debts.Remove(debt);
            await _context.SaveChangesAsync(cancellationToken);

            // The entity has been deleted successfully, adjust the summary value based on debt type.
            await AdjustSummaryAsync(debt, debt.Amount, cancellationToken);

            // Commit the transaction, finish the operation.
            await transaction.CommitAsync(cancellationToken);
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
                    debt.IsDeleted = true;

                    // Adjust the stats.
                    await AdjustStatsAsync(debt, _statsService, false);

                    // Save changes and commit the transaction again, finish the operation.
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }
    #endregion

    #region PrivateMethods
    /// <summary>
    /// Adjusts the created/modified summary related data for statistics.
    /// </summary>
    /// <param name="debt">
    /// The instance of the <see cref="Debt"/> entity that causes the incrementing.
    /// </param>
    /// <param name="amountToIncrement">
    /// A <see langword="long"/> value indicating the amount to increment.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    private async Task AdjustSummaryAsync(
            Debt debt,
            long amountToIncrement,
            CancellationToken cancellationToken = default)
    {
        DateOnly summaryDate = DateOnly.FromDateTime(debt.StatsDateTime);
        if (debt.Type == DebtType.Incurrence)
        {
            await _summaryService.IncrementDebtIncurredAmountAsync(amountToIncrement, summaryDate, cancellationToken);
        }
        else
        {
            await _summaryService.IncrementDebtPaidAmountAsync(amountToIncrement, summaryDate, cancellationToken);
        }
    }

    /// <summary>
    /// Converts the exception which is thrown by the database during the creating or updating operation into an
    /// instance of <see cref="CoreException"/> .
    /// </summary>
    /// <param name="exception">
    /// An instance of the <see cref="DbUpdateException"/> class, contanining the details of the error.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="CoreException"/> class, representing the converted exception (when successful) or
    /// <see langword="null"/> (if failure).
    /// </returns>
    private CoreException? TryHandleDbUpdateException(DbUpdateException exception)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is null)
        {
            return null;
        }

        if (handledResult.IsConcurrencyConflict)
        {
            return new ConcurrencyException();
        }

        if (handledResult.IsForeignKeyConstraintViolation &&
            handledResult.ViolatedPropertyName == nameof(Debt.CustomerId))
        {
            return OperationException.NotFound(
                new object[] { nameof(DebtUpsertRequestDto.CustomerId) },
                DisplayNames.Customer
            );
        }

        return null;
    }
    #endregion

    #region StaticMethods
    /// <summary>
    /// Adjust the value of the <c>CachedRemainingDebtAmount</c> property of the <see cref="Customer"/> entity mapped to
    /// the given <paramref name="debt"/> entity.
    /// </summary>
    /// <param name="debt">
    /// An instance of the <see cref="Debt"/> entity, which has the related <see cref="Customer"/> entity to modify.
    /// </param>
    /// <param name="amountDifference">
    /// A <see langword="long"/> value indicating the amount difference to adjust.
    /// </param>
    private static void AdjustCustomerRemainingDebtAmount(Debt debt, long amountDifference)
    {
        if (debt.Type == DebtType.Incurrence)
        {
            debt.Customer.CachedRemainingDebtAmount += amountDifference;
            return;
        }

        debt.Amount -= amountDifference;
    }
    #endregion
}