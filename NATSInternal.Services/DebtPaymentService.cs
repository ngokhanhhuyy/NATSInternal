namespace NATSInternal.Services;

/// <inheritdoc cref="IDebtPaymentService" />
internal class DebtPaymentService
    :
        DebtAbstractService<DebtPayment, DebtPaymentUpdateHistory, DebtPaymentListRequestDto,
            DebtPaymentUpsertRequestDto, DebtPaymentListResponseDto,
                DebtPaymentBasicResponseDto, DebtPaymentDetailResponseDto,
                DebtPaymentUpdateHistoryResponseDto, DebtPaymentUpdateHistoryDataDto,
                DebtPaymentListAuthorizationResponseDto, DebtPaymentAuthorizationResponseDto>,
        IDebtPaymentService
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService<
        DebtPayment,
        DebtPaymentUpdateHistory> _statsService;
    private readonly IUpdateHistoryService<
        DebtPayment,
        DebtPaymentUpdateHistory,
        DebtPaymentUpdateHistoryDataDto> _updateHistoryService;

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
            updateHistoryService,
            monthYearService,
            DebtType.DebtPayment)
    {
        _context = context;
        _statsService = statsService;
        _authorizationService = authorizationService;
        _updateHistoryService = updateHistoryService;
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

    /// <inheritdoc />
    protected override DbSet<DebtPayment> GetRepository(DatabaseContext context)
    {
        return context.DebtPayments;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override IQueryable<DebtPayment> FilterByMonthYearListQuery(
            IQueryable<DebtPayment> query,
            DateTime startingDateTime,
            DateTime endingDateTime)
    {
        return query
            .Where(dp => dp.PaidDateTime >= startingDateTime)
            .Where(dp => dp.PaidDateTime < endingDateTime);
    }

    /// <inheritdoc />
    protected override DebtPaymentBasicResponseDto InitializeBasicResponseDto(
            DebtPayment debtPayment)
    {
        return new DebtPaymentBasicResponseDto(
            debtPayment,
            _authorizationService.GetDebtPaymentAuthorization(debtPayment));
    }

    /// <inheritdoc />
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
    protected override DebtPaymentListAuthorizationResponseDto
        InitializeListAuthorizationResponseDto(IAuthorizationInternalService service)
    {
        return service.GetDebtPaymentListAuthorization();
    }

    /// <inheritdoc />
    protected override DebtPaymentAuthorizationResponseDto
        InitializeAuthorizationResponseDto(
            IAuthorizationInternalService service,
            DebtPayment debtPayment)
    {
        return service.GetDebtPaymentAuthorization(debtPayment);
    }

    /// <inheritdoc />
    protected override DebtPaymentUpdateHistoryDataDto
        InitializeUpdateHistoryDataDto(DebtPayment entity)
    {
        return new DebtPaymentUpdateHistoryDataDto(entity);
    }

    /// <inheritdoc />
    protected override bool CanAccessUpdateHistory(IAuthorizationInternalService service)
    {
        return service.CanAccessDebtPaymentUpdateHistories();
    }

    /// <inheritdoc />
    protected override bool CanSetStatsDateTime(IAuthorizationInternalService service)
    {
        return service.CanSetDebtPaymentPaidDateTime();
    }
}