namespace NATSInternal.Services;

/// <inheritdoc cref="IDebtPaymentService" />
internal class DebtPaymentService
    :
        DebtAbstractService<DebtPayment, DebtPaymentUpdateHistory,
            DebtPaymentListRequestDto, DebtPaymentUpsertRequestDto,
            DebtPaymentListResponseDto, DebtPaymentBasicResponseDto,
            DebtPaymentDetailResponseDto, DebtPaymentUpdateHistoryResponseDto,
            DebtPaymentUpdateHistoryDataDto, DebtPaymentListAuthorizationResponseDto,
            DebtPaymentAuthorizationResponseDto>,
        IDebtPaymentService
{
    private readonly IAuthorizationInternalService _authorizationService;

    public DebtPaymentService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<DebtPayment, DebtPaymentUpdateHistory> statsService)
        : base(context, authorizationService, statsService)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<DebtPaymentListResponseDto> GetListAsync(
            DebtPaymentListRequestDto requestDto)
    {
        EntityListDto<DebtPayment> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new DebtPaymentListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items
                .Select(dp =>
                {
                    DebtPaymentAuthorizationResponseDto authorization;
                    authorization = _authorizationService.GetDebtPaymentAuthorization(dp);
                    return new DebtPaymentBasicResponseDto(dp, authorization);
                }).ToList(),
            MonthYearOptions = await GenerateMonthYearOptions(),
            Authorization = _authorizationService.GetDebtPaymentListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<DebtPaymentDetailResponseDto> GetDetailAsync(int id)
    {
        DebtPayment debtPayment = await GetEntityAsync(id);
        DebtPaymentAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetDebtPaymentAuthorization(debtPayment);

        return new DebtPaymentDetailResponseDto(debtPayment, authorization);
    }

    /// <inheritdoc />
    protected override DbSet<DebtPayment> GetRepository(DatabaseContext context)
    {
        return context.DebtPayments;
    }

    /// <inheritdoc />
    protected override DebtPaymentUpdateHistoryDataDto
        InitializeUpdateHistoryDataDto(DebtPayment debtPayment)
    {
        return new DebtPaymentUpdateHistoryDataDto(debtPayment);
    }

    /// <inheritdoc />
    protected override bool CanAccessUpdateHistories(IAuthorizationInternalService service)
    {
        return service.CanAccessDebtPaymentUpdateHistories();
    }

    /// <inheritdoc />
    protected override bool CanEdit(
            DebtPayment debtPayment,
            IAuthorizationInternalService service)
    {
        return service.CanEditDebtPayment(debtPayment);
    }

    /// <inheritdoc />
    protected override bool CanSetStatsDateTime(IAuthorizationInternalService service)
    {
        return service.CanSetDebtPaymentPaidDateTime();
    }

    /// <inheritdoc />
    protected override bool CanDelete(
            DebtPayment debtPayment,
            IAuthorizationInternalService service)
    {
        return service.CanDeleteDebtPayment(debtPayment);
    }

    /// <inheritdoc />
    protected override async Task AdjustStatsAsync(
            DebtPayment debtPayment,
            IStatsInternalService<DebtPayment, DebtPaymentUpdateHistory> service,
            bool isIncrement)
    {
        long amountToIncrement = isIncrement ? debtPayment.Amount : -debtPayment.Amount;
        DateOnly date = DateOnly.FromDateTime(debtPayment.StatsDateTime);
        await service.IncrementDebtPaidAmountAsync(amountToIncrement, date);
    }
}
