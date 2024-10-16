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
    private readonly IAuthorizationInternalService _authorizationService;

    public DebtIncurrenceService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<DebtIncurrence, DebtIncurrenceUpdateHistory> statsService)
        : base(context, authorizationService, statsService)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<DebtIncurrenceListResponseDto> GetListAsync(
            DebtIncurrenceListRequestDto requestDto)
    {
        EntityListDto<DebtIncurrence> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new DebtIncurrenceListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items
                .Select(di =>
                {
                    DebtIncurrenceAuthorizationResponseDto authorization;
                    authorization = _authorizationService.GetDebtIncurrenceAuthorization(di);
                    return new DebtIncurrenceBasicResponseDto(di, authorization);
                }).ToList(),
            MonthYearOptions = await GenerateMonthYearOptions(),
            Authorization = _authorizationService.GetDebtIncurrenceListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<DebtIncurrenceDetailResponseDto> GetDetailAsync(int id)
    {
        DebtIncurrence debtIncurrence = await GetEntityAsync(id);
        DebtIncurrenceAuthorizationResponseDto authorization = _authorizationService
            .GetDebtIncurrenceAuthorization(debtIncurrence);

        return new DebtIncurrenceDetailResponseDto(debtIncurrence, authorization);
    }

    /// <inheritdoc />
    protected override DbSet<DebtIncurrence> GetRepository(DatabaseContext context)
    {
        return context.DebtIncurrences;
    }

    /// <inheritdoc />
    protected override DebtIncurrenceUpdateHistoryDataDto InitializeUpdateHistoryDataDto(
            DebtIncurrence debtIncurrence)
    {
        return new DebtIncurrenceUpdateHistoryDataDto(debtIncurrence);
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
        return service.CanSetDebtIncurrenceStatsDateTime();
    }

    /// <inheritdoc />
    protected override bool CanAccessUpdateHistories(IAuthorizationInternalService service)
    {
        return service.CanAccessDebtIncurrenceUpdateHistories();
    }

    /// <inheritdoc />
    protected override bool CanDelete(
            DebtIncurrence debtIncurrence,
            IAuthorizationInternalService service)
    {
        return service.CanDeleteDebtIncurrence(debtIncurrence);
    }

    /// <inheritdoc />
    protected override async Task AdjustStatsAsync(
            DebtIncurrence debtIncurrence,
            IStatsInternalService<DebtIncurrence, DebtIncurrenceUpdateHistory> service,
            bool isIncrement)
    {
        long amountToIncrement = isIncrement ? debtIncurrence.Amount : -debtIncurrence.Amount;
        DateOnly date = DateOnly.FromDateTime(debtIncurrence.StatsDateTime);
        await service.IncrementDebtIncurredAmountAsync(amountToIncrement, date);
    }
}
