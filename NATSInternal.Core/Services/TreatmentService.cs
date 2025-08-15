namespace NATSInternal.Core.Services;

internal class TreatmentService
    : ExportProductAbstractService<
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
        TreatmentItemUpdateHistoryDataDto,
        TreatmentUpdateHistoryDataDto,
        TreatmentCreatingAuthorizationResponseDto,
        TreatmentExistingAuthorizationResponseDto>,
    ITreatmentService
{
    public TreatmentService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IMultiplePhotosService<Treatment, TreatmentPhoto> photoService,
            IStatsInternalService statsService)
        : base(context, authorizationService, photoService, statsService)
    {
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
                    GetExistingAuthorization(treatment)))
                .ToList(),
        };
    }

    /// <inheritdoc />
    public async Task<TreatmentDetailResponseDto> GetDetailAsync(int id)
    {
        Treatment treatment = await GetEntityAsync(id);

        return new TreatmentDetailResponseDto(treatment, GetExistingAuthorization(treatment));
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
    protected override async Task AdjustStatsAsync(
            Treatment treatment,
            IStatsInternalService statsService,
            bool isIncrementing)
    {
        DateOnly statsDate = DateOnly.FromDateTime(treatment.StatsDateTime);
        await statsService.IncrementRetailGrossRevenueAsync(
            isIncrementing ? treatment.AmountBeforeVat : -treatment.AmountBeforeVat,
            statsDate);
        await statsService.IncrementVatCollectedAmountAsync(
            isIncrementing ? treatment.VatAmount : -treatment.VatAmount,
            statsDate);
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
    protected override void AssignExistingEntityProperty(
            Treatment entity,
            TreatmentUpsertRequestDto requestDto)
    {
        base.AssignExistingEntityProperty(entity, requestDto);
        entity.ServiceAmountBeforeVat = requestDto.ServiceAmountBeforeVat;
        entity.ServiceVatAmount = requestDto.ServiceVatAmount;
        entity.TherapistId = requestDto.TherapistId;
    }
}
