namespace NATSInternal.Services.Dtos;

public class DebtPaymentInitialResponseDto
        : IHasStatsInitialResponseDto<DebtPaymentCreatingAuthorizationResponseDto>
{
    public string DisplayName { get; } = DisplayNames.Consultant;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; set; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; set; }
    public required DebtPaymentCreatingAuthorizationResponseDto CreatingAuthorization { get; set; }
    public bool CreatingPermission => CreatingAuthorization != null;
}