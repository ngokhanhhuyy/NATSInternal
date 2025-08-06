namespace NATSInternal.Core.Dtos;

public class TreatmentInitialResponseDto
        : IHasStatsInitialResponseDto<TreatmentCreatingAuthorizationResponseDto>
{
    public string DisplayName => DisplayNames.Treatment;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; init; }
    public required TreatmentCreatingAuthorizationResponseDto CreatingAuthorization
    {
        get;
        init;
    }
    public bool CreatingPermission => CreatingAuthorization != null;

}
