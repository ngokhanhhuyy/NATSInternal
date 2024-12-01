namespace NATSInternal.Services;

/// <inheritdoc cref="IDebtIncurrenceService" />
internal class DebtIncurrenceService
    :
        DebtAbstractService<DebtIncurrence, DebtIncurrenceUpdateHistory,
            DebtIncurrenceListRequestDto, DebtIncurrenceUpsertRequestDto,
            DebtIncurrenceListResponseDto, DebtIncurrenceBasicResponseDto,
            DebtIncurrenceDetailResponseDto, DebtIncurrenceUpdateHistoryResponseDto,
            DebtIncurrenceUpdateHistoryDataDto, DebtIncurrenceCreatingAuthorizationResponseDto,
            DebtIncurrenceExistingAuthorizationResponseDto>,
        IDebtIncurrenceService
{

    public DebtIncurrenceService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService statsService)
        : base(context, authorizationService, statsService)
    {
    }

    /// <inheritdoc />
    public async Task<DebtIncurrenceListResponseDto> GetListAsync(
            DebtIncurrenceListRequestDto requestDto)
    {
        EntityListDto<DebtIncurrence> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new DebtIncurrenceListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(di =>
                {
                    DebtIncurrenceExistingAuthorizationResponseDto authorization;
                    authorization = GetExistingAuthorization(di);
                    return new DebtIncurrenceBasicResponseDto(di, authorization);
                }).ToList()
                ?? new List<DebtIncurrenceBasicResponseDto>(),
        };
    }

    /// <inheritdoc />
    public async Task<DebtIncurrenceDetailResponseDto> GetDetailAsync(int id)
    {
        DebtIncurrence debtIncurrence = await GetEntityAsync(id);

        DebtIncurrenceExistingAuthorizationResponseDto authorization;
        authorization = GetExistingAuthorization(debtIncurrence);

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
    protected override async Task AdjustStatsAsync(
            DebtIncurrence debtIncurrence,
            IStatsInternalService service,
            bool isIncrement)
    {
        long amountToIncrement = isIncrement ? debtIncurrence.Amount : -debtIncurrence.Amount;
        DateOnly date = DateOnly.FromDateTime(debtIncurrence.StatsDateTime);
        await service.IncrementDebtIncurredAmountAsync(amountToIncrement, date);
    }
}