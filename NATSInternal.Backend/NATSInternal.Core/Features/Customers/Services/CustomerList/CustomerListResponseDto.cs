using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Customers;

public class CustomerListResponseDto : IListResponseDto<CustomerBasicResponseDto>
{
    #region Constructors
    internal CustomerListResponseDto(List<CustomerBasicResponseDto> items, int itemCount, int pageCount)
    {
        Items = items;
        ItemCount = itemCount;
        PageCount = pageCount;
    }
    #endregion
    
    #region Properties
    public List<CustomerBasicResponseDto> Items { get; }
    public int ItemCount { get; }
    public int PageCount { get; }
    #endregion
}