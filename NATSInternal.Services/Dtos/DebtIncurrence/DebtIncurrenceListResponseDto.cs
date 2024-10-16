namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceListResponseDto : IFinancialEngageableListResponseDto<
        DebtIncurrenceBasicResponseDto,
        DebtIncurrenceAuthorizationResponseDto,
        DebtIncurrenceListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<DebtIncurrenceBasicResponseDto> Items { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public DebtIncurrenceListAuthorizationResponseDto Authorization { get; set; }
}