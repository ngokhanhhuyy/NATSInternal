namespace NATSInternal.Core.Services;

/// <inheritdoc cref="IDebtIncurrenceService" />
internal class DebtIncurrenceService
    :
        DebtAbstractService<Debt, DebtUpdateHistory,
            DebtIncurrenceListRequestDto, DebtIncurrenceUpsertRequestDto,
            DebtIncurrenceListResponseDto, DebtBasicResponseDto,
            DebtDetailResponseDto, DebtUpdateHistoryResponseDto,
            DebtIncurrenceUpdateHistoryDataDto, DebtCreatingAuthorizationResponseDto,
            DebtExistingAuthorizationResponseDto>,
        IDebtIncurrenceService
{

    public DebtIncurrenceService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            ISummaryInternalService statsService)
        : base(context, authorizationService, statsService)
    {
    }

    /// <inheritdoc />
    public async Task<DebtIncurrenceListResponseDto> GetListAsync(
            DebtIncurrenceListRequestDto requestDto)
    {
        EntityListDto<Debt> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new DebtIncurrenceListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(di =>
                {
                    DebtExistingAuthorizationResponseDto authorization;
                    authorization = GetExistingAuthorization(di);
                    return new DebtBasicResponseDto(di, authorization);
                }).ToList()
                ?? new List<DebtBasicResponseDto>(),
        };
    }

    /// <inheritdoc />
    public async Task<DebtDetailResponseDto> GetDetailAsync(int id)
    {
        Debt debtIncurrence = await GetEntityAsync(id);

        DebtExistingAuthorizationResponseDto authorization;
        authorization = GetExistingAuthorization(debtIncurrence);

        return new DebtDetailResponseDto(debtIncurrence, authorization);
    }

    /// <inheritdoc />
    protected override DbSet<Debt> GetRepository(DatabaseContext context)
    {
        return context.DebtIncurrences;
    }

    /// <inheritdoc />
    protected override DebtIncurrenceUpdateHistoryDataDto InitializeUpdateHistoryDataDto(
            Debt debtIncurrence)
    {
        return new DebtIncurrenceUpdateHistoryDataDto(debtIncurrence);
    }

    /// <inheritdoc />
    protected override async Task AdjustStatsAsync(
            Debt debtIncurrence,
            ISummaryInternalService service,
            bool isIncrement)
    {
        long amountToIncrement = isIncrement ? debtIncurrence.Amount : -debtIncurrence.Amount;
        DateOnly date = DateOnly.FromDateTime(debtIncurrence.StatsDateTime);
        await service.IncrementDebtIncurredAmountAsync(amountToIncrement, date);
    }

    /// <inheritdoc />
    protected override void AdjustCustomerCachedDebtAmount(
            Customer customer,
            long differentAmount)
    {
        customer.CachedIncurredDebtAmount -= differentAmount;
    }
}