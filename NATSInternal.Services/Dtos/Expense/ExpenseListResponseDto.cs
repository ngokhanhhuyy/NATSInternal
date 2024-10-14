namespace NATSInternal.Services.Dtos;

public class ExpenseListResponseDto : IFinancialEngageableListResponseDto<
    ExpenseBasicResponseDto,
    ExpenseAuthorizationResponseDto,
    ExpenseListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<ExpenseBasicResponseDto> Items { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public ExpenseListAuthorizationResponseDto Authorization { get; set; }
}