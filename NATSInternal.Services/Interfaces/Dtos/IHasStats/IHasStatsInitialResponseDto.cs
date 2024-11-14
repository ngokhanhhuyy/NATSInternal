namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasStatsInitialResponseDto<TCreatingAuthorization>
        :
            IUpsertableInitialResponseDto,
            ISortableInitialResponseDto
        where TCreatingAuthorization : IHasStatsCreatingAuthorizationResponseDto
{
    ListMonthYearOptionsResponseDto ListMonthYearOptions { get; }
    TCreatingAuthorization CreatingAuthorization { get; }
}