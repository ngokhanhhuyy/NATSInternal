using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyListResponseDto : IListResponseDto<SupplyBasicResponseDto>
{
    #region Constructors
    public SupplyListResponseDto(List<SupplyBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public List<SupplyBasicResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}