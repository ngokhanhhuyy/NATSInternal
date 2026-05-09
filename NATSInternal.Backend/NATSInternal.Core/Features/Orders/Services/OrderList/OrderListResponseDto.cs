using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Orders;

public class OrderListResponseDto : IListResponseDto<OrderBasicResponseDto>
{
    #region Constructors
    public OrderListResponseDto(List<OrderBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public List<OrderBasicResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}