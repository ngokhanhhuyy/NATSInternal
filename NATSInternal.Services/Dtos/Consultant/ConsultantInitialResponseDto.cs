namespace NATSInternal.Services.Dtos;

public class ConsultantInitialResponseDto
        : IHasStatsInitialResponseDto<ConsultantCreatingAuthorizationResponseDto>
{
    public string DisplayName { get; } = DisplayNames.Consultant;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; init; }
    public required ConsultantCreatingAuthorizationResponseDto CreatingAuthorization
    {
        get;
        init;
    }
    public bool CreatingPermission => CreatingAuthorization != null;
}