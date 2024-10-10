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
    private readonly IAuthorizationInternalService _authorizationService;

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
            monthYearService)
    {
        _authorizationService = authorizationService;
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
            DebtPayment debtPayment,
            IAuthorizationInternalService service)
    {
        return new DebtPaymentBasicResponseDto(
            debtPayment,
            service.GetDebtPaymentAuthorization(debtPayment));
    }

    /// <inheritdoc />
    protected override DebtPaymentDetailResponseDto InitializeDetailResponseDto(
            DebtPayment debtPayment,
            IAuthorizationInternalService service,
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

    /// <inheritdoc />
    protected override bool CanDelete(IAuthorizationInternalService service)
    {
        return service.CanDeleteDebtPayment();
    }
}