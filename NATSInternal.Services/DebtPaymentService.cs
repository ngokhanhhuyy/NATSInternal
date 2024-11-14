namespace NATSInternal.Services;

/// <inheritdoc cref="IDebtPaymentService" />
internal class DebtPaymentService
    :
        DebtAbstractService<DebtPayment, DebtPaymentUpdateHistory,
            DebtPaymentListRequestDto, DebtPaymentUpsertRequestDto,
            DebtPaymentListResponseDto, DebtPaymentBasicResponseDto,
            DebtPaymentDetailResponseDto, DebtPaymentUpdateHistoryResponseDto,
            DebtPaymentUpdateHistoryDataDto, DebtPaymentCreatingAuthorizationResponseDto,
            DebtPaymentExistingAuthorizationResponseDto>,
        IDebtPaymentService
{

    public DebtPaymentService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<DebtPayment, DebtPaymentUpdateHistory> statsService)
        : base(context, authorizationService, statsService)
    {
    }

    /// <inheritdoc />
    public async Task<DebtPaymentListResponseDto> GetListAsync(
            DebtPaymentListRequestDto requestDto)
    {
        EntityListDto<DebtPayment> entityListDto = await GetListOfEntitiesAsync(requestDto);

        return new DebtPaymentListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(debtPayment =>
                {
                    DebtPaymentExistingAuthorizationResponseDto authorization;
                    authorization = GetExistingAuthorization(debtPayment);

                    return new DebtPaymentBasicResponseDto(debtPayment, authorization);
                }).ToList()
                ?? new List<DebtPaymentBasicResponseDto>(),
        };
    }

    /// <inheritdoc />
    public async Task<DebtPaymentDetailResponseDto> GetDetailAsync(int id)
    {
        DebtPayment debtPayment = await GetEntityAsync(id);

        DebtPaymentExistingAuthorizationResponseDto authorization;
        authorization = GetExistingAuthorization(debtPayment);

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
