namespace NATSInternal.Services.Dtos;

public class OrderListResponseDto : IFinancialEngageableListResponseDto<
        OrderBasicResponseDto,
        OrderExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<OrderBasicResponseDto> Items { get; set; }
}