namespace NATSInternal.Services.Dtos;

public class OrderListResponseDto : IFinancialEngageableListResponseDto<
        OrderBasicResponseDto,
        OrderAuthorizationResponseDto,
        OrderListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<OrderBasicResponseDto> Items { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public OrderListAuthorizationResponseDto Authorization { get; set; }
}