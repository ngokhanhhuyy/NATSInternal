namespace NATSInternal.Services;

/// <inheritdoc cref="IDebtIncurrenceService" />
internal class DebtIncurrenceService
    :
        DebtAbstractService<DebtIncurrence, DebtIncurrenceUpdateHistory,
            DebtIncurrenceListRequestDto, DebtIncurrenceUpsertRequestDto,
            DebtIncurrenceListResponseDto, DebtIncurrenceBasicResponseDto,
            DebtIncurrenceDetailResponseDto, DebtIncurrenceUpdateHistoryResponseDto,
            DebtIncurrenceUpdateHistoryDataDto, DebtIncurrenceListAuthorizationResponseDto,
            DebtIncurrenceAuthorizationResponseDto>,
        IDebtIncurrenceService
{
    public DebtIncurrenceService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<DebtIncurrence, DebtIncurrenceUpdateHistory> statsService,
            IUpdateHistoryService<DebtIncurrence, DebtIncurrenceUpdateHistory, DebtIncurrenceUpdateHistoryDataDto> updateHistoryService,
            IMonthYearService<DebtIncurrence, DebtIncurrenceUpdateHistory> monthYearService)
        : base(
            context,
            authorizationService,
            statsService,
            updateHistoryService,
            monthYearService)
    {
    }
    
    /// <inheritdoc />
    protected override DbSet<DebtIncurrence> GetRepository(DatabaseContext context)
    {
        return context.DebtIncurrences;
    }

    /// <inheritdoc />
    protected override IOrderedQueryable<DebtIncurrence> SortListQuery(
            IQueryable<DebtIncurrence> query,
            DebtIncurrenceListRequestDto requestDto)
    {
        switch (requestDto.OrderByField)
        {
            case nameof(DebtIncurrenceListRequestDto.FieldOptions.Amount):
                return requestDto.OrderByAscending
                    ? query.OrderBy(dp => dp.Amount).ThenBy(dp => dp.IncurredDateTime)
                    : query.OrderByDescending(dp => dp.Amount)
                        .ThenByDescending(dp => dp.IncurredDateTime);
            default:
                return requestDto.OrderByAscending
                    ? query.OrderBy(dp => dp.IncurredDateTime).ThenBy(dp => dp.Amount)
                    : query.OrderByDescending(dp => dp.IncurredDateTime)
                        .ThenByDescending(dp => dp.Amount);
        }
    }

    /// <inheritdoc />
    protected override IQueryable<DebtIncurrence> FilterByMonthYearListQuery(
            IQueryable<DebtIncurrence> query,
            DateTime startingDateTime,
            DateTime endingDateTime)
    {
        return query
            .Where(dp => dp.IncurredDateTime >= startingDateTime)
            .Where(dp => dp.IncurredDateTime < endingDateTime);
    }

    /// <inheritdoc />
    protected override DebtIncurrenceBasicResponseDto InitializeBasicResponseDto(
            DebtIncurrence debtIncurrence,
            IAuthorizationInternalService authorizationService)
    {
        return new DebtIncurrenceBasicResponseDto(
            debtIncurrence,
            authorizationService.GetDebtIncurrenceAuthorization(debtIncurrence));
    }

    /// <inheritdoc />
    protected override DebtIncurrenceDetailResponseDto InitializeDetailResponseDto(
            DebtIncurrence debtIncurrence,
            IAuthorizationInternalService authorizationService,
            bool shouldIncludeUpdateHistories)
    {
        return new DebtIncurrenceDetailResponseDto(
            debtIncurrence,
            authorizationService.GetDebtIncurrenceAuthorization(debtIncurrence),
            mapUpdateHistories: shouldIncludeUpdateHistories);
    }

    /// <inheritdoc />
    protected override DebtIncurrenceListAuthorizationResponseDto
        InitializeListAuthorizationResponseDto(IAuthorizationInternalService service)
    {
        return service.GetDebtIncurrenceListAuthorization();
    }

    /// <inheritdoc />
    protected override DebtIncurrenceAuthorizationResponseDto
        InitializeAuthorizationResponseDto(
            IAuthorizationInternalService service,
            DebtIncurrence debtIncurrence)
    {
        return service.GetDebtIncurrenceAuthorization(debtIncurrence);
    }

    /// <inheritdoc />
    protected override DebtIncurrenceUpdateHistoryDataDto
        InitializeUpdateHistoryDataDto(DebtIncurrence debtIncurrence)
    {
        return new DebtIncurrenceUpdateHistoryDataDto(debtIncurrence);
    }

    /// <inheritdoc />
    protected override bool CanAccessUpdateHistory(IAuthorizationInternalService service)
    {
        return service.CanAccessDebtIncurrenceUpdateHistories();
    }

    /// <inheritdoc />
    protected override bool CanEdit(
            DebtIncurrence debtIncurrence,
            IAuthorizationInternalService service)
    {
        return service.CanEditDebtIncurrence(debtIncurrence);
    }

    /// <inheritdoc />
    protected override bool CanSetStatsDateTime(IAuthorizationInternalService service)
    {
        return service.CanSetDebtIncurrenceIncurredDateTime();
    }

    /// <inheritdoc />
    protected override bool CanDelete(IAuthorizationInternalService service)
    {
        return service.CanDeleteDebtIncurrence();
    }

    /// <inheritdoc />
    protected override async Task IncrementStatsAsync(
            long amount,
            DateOnly date,
            IStatsInternalService<DebtIncurrence, DebtIncurrenceUpdateHistory> service)
    {
        await service.IncrementDebtIncurredAmountAsync(amount, date);
    }
}
