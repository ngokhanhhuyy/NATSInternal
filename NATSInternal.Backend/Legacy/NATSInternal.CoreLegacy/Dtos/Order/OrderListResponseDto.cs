namespace NATSInternal.Core.Dtos;

public class OrderListResponseDto : IHasStatsResponseDto<
        OrderBasicResponseDto,
        OrderExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<OrderBasicResponseDto> Items { get; set; }
}