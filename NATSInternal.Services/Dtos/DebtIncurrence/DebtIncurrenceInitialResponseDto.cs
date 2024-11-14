namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceInitialResponseDto
        : IHasStatsInitialResponseDto<DebtIncurrenceCreatingAuthorizationResponseDto>
{
    public string DisplayName { get; } = DisplayNames.DebtIncurrence;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; init; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; init; }
    public required DebtIncurrenceCreatingAuthorizationResponseDto CreatingAuthorization
    {
        get;
        init;
    }
    
    public bool CreatingPermission => CreatingAuthorization != null;
}