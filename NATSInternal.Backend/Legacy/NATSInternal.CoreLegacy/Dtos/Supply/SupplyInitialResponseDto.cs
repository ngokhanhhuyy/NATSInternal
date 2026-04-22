namespace NATSInternal.Core.Dtos;

public class SupplyInitialResponseDto
        : IHasStatsInitialResponseDto<SupplyCreatingAuthorizationResponseDto>
{
    public string DisplayName => DisplayNames.Supply;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; init; }
    public required SupplyCreatingAuthorizationResponseDto CreatingAuthorization { get; init; }
    public bool CreatingPermission => CreatingAuthorization != null;
}
