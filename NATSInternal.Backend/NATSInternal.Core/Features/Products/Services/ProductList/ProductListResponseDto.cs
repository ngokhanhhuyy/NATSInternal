using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Products;

public class ProductListResponseDto : IListResponseDto<ProductBasicResponseDto>
{
    #region Constructors
    internal ProductListResponseDto(List<ProductBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public List<ProductBasicResponseDto> Items { get; set; }
    public int PageCount { get; set; }
    public int ItemCount { get; set; }
    #endregion
}
