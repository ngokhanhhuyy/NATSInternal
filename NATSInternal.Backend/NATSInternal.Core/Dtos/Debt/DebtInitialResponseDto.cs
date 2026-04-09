namespace NATSInternal.Core.Dtos;

public class DebtInitialResponseDto : IHasStatsInitialResponseDto<DebtCreatingAuthorizationResponseDto>
{
    public string DisplayName { get; } = DisplayNames.DebtIncurrence;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; init; }
    public required DebtCreatingAuthorizationResponseDto CreatingAuthorization { get; init; }
    
    public bool CreatingPermission => CreatingAuthorization != null;
}