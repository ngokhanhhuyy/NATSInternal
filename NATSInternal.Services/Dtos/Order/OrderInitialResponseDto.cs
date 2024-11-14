namespace NATSInternal.Services.Dtos;

public class OrderInitialResponseDto
        : IHasStatsInitialResponseDto<OrderCreatingAuthorizationResponseDto>
{
    public string DisplayName { get; } = DisplayNames.Order;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; set; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; set; }
    public required OrderCreatingAuthorizationResponseDto CreatingAuthorization { get; set; }
    public bool CreatingPermission => CreatingAuthorization != null;
}