namespace NATSInternal.Core.Dtos;

public class ExpenseInitialResponseDto
        : IHasStatsInitialResponseDto<ExpenseCreatingAuthorizationResponseDto>
{
    public string DisplayName { get; } = DisplayNames.Expense;
    public required ListSortingOptionsResponseDto ListSortingOptions { get; set; }
    public required ListMonthYearOptionsResponseDto ListMonthYearOptions { get; set; }
    public required ExpenseCreatingAuthorizationResponseDto CreatingAuthorization { get; set; }
    public bool CreatingPermission => CreatingAuthorization != null;
}