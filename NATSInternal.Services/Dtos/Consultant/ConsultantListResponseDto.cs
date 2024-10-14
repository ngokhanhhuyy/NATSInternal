namespace NATSInternal.Services.Dtos;

public class ConsultantListResponseDto : IFinancialEngageableListResponseDto<
        ConsultantBasicResponseDto,
        ConsultantAuthorizationResponseDto,
        ConsultantListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<ConsultantBasicResponseDto> Items { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public ConsultantListAuthorizationResponseDto Authorization { get; set; }
}