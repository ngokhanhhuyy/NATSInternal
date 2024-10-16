namespace NATSInternal.Services;

internal class TreatmentService
    : ProductExportableAbstractService<
        Treatment,
        TreatmentItem,
        TreatmentPhoto,
        TreatmentUpdateHistory,
        TreatmentListRequestDto,
        TreatmentUpsertRequestDto,
        TreatmentItemRequestDto,
        TreatmentPhotoRequestDto,
        TreatmentListResponseDto,
        TreatmentBasicResponseDto,
        TreatmentDetailResponseDto,
        TreatmentItemResponseDto,
        TreatmentPhotoResponseDto,
        TreatmentUpdateHistoryResponseDto,
        TreatmentUpdateHistoryDataDto,
        TreatmentListAuthorizationResponseDto,
        TreatmentAuthorizationResponseDto>,
    ITreatmentService
{
    private readonly IAuthorizationInternalService _authorizationService;

    public TreatmentService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IPhotoService<Treatment, TreatmentPhoto> photoService,
            IStatsInternalService<Treatment, TreatmentUpdateHistory> statsService)
        : base(context, authorizationService, photoService, statsService)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<TreatmentListResponseDto> GetListAsync(
            TreatmentListRequestDto requestDto)
    {
        EntityListDto<Treatment> listDto = await GetListOfEntitiesAsync(requestDto);

        return new TreatmentListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items
                .Select(treatment => new TreatmentBasicResponseDto(
                    treatment,
                    _authorizationService.GetTreatmentAuthorization(treatment)))
                .ToList(),
            MonthYearOptions = await GenerateMonthYearOptions(),
            Authorization = _authorizationService.GetTreatmentListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<TreatmentDetailResponseDto> GetDetailAsync(int id)
    {
        Treatment treatment = await GetEntityAsync(id);
        TreatmentAuthorizationResponseDto authorization = _authorizationService
            .GetTreatmentAuthorization(treatment);

        return new TreatmentDetailResponseDto(treatment, authorization);
    }

    /// <inheritdoc />
    protected override DbSet<Treatment> GetRepository(DatabaseContext context)
    {
        return context.Treatments;
    }

    /// <inheritdoc />
    protected override DbSet<TreatmentItem> GetItemRepository(DatabaseContext context)
    {
        return context.TreatmentItems;
    }

    /// <inheritdoc />
    protected override TreatmentUpdateHistoryDataDto
        InitializeUpdateHistoryDataDto(Treatment treatment)
    {
        return new TreatmentUpdateHistoryDataDto(treatment);
    }

    /// <inheritdoc />
    protected override bool CanAccessUpdateHistories(IAuthorizationInternalService service)
    {
        return service.CanAccessTreatmentUpdateHistories();
    }

    /// <inheritdoc />
    protected override bool CanSetStatsDateTime(IAuthorizationInternalService service)
    {
        return service.CanSetTreatmentStatsDateTime();
    }

    /// <inheritdoc />
    protected override bool CanEdit(Treatment treatment, IAuthorizationInternalService service)
    {
        return service.CanEditTreatment(treatment);
    }

    /// <inheritdoc />
    protected override bool CanDelete(Treatment treatment, IAuthorizationInternalService service)
    {
        return service.CanDeleteTreatment(treatment);
    }

    /// <inheritdoc />
    protected override async Task AdjustStatsAsync(
            Treatment treatment,
            IStatsInternalService<Treatment, TreatmentUpdateHistory> statsService,
            bool isIncrementing)
    {
        DateOnly paidDate = DateOnly.FromDateTime(treatment.StatsDateTime);
        await statsService.IncrementRetailGrossRevenueAsync(
            isIncrementing ? treatment.AmountBeforeVat : -treatment.AmountBeforeVat,
            paidDate);
        await statsService.IncrementVatCollectedAmountAsync(
            isIncrementing ? treatment.VatAmount : -treatment.VatAmount,
            paidDate);
    }

    /// <inheritdoc />
    protected override void AssignNewEntityProperties(
            Treatment entity,
            TreatmentUpsertRequestDto requestDto)
    {
        base.AssignNewEntityProperties(entity, requestDto);
        entity.ServiceAmountBeforeVat = requestDto.ServiceAmountBeforeVat;
        entity.ServiceVatAmount = requestDto.ServiceVatAmount;
        entity.TherapistId = requestDto.TherapistId;
    }

    /// <inheritdoc />
    protected override void AssignExstingEntityProperty(
            Treatment entity,
            TreatmentUpsertRequestDto requestDto)
    {
        base.AssignExstingEntityProperty(entity, requestDto);
        entity.ServiceAmountBeforeVat = requestDto.ServiceAmountBeforeVat;
        entity.ServiceVatAmount = requestDto.ServiceVatAmount;
        entity.TherapistId = requestDto.TherapistId;
    }
}
