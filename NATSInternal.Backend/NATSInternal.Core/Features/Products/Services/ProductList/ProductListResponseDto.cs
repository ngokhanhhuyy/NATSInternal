using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Products;

public class ProductListResponseDto : IListResponseDto<ProductBasicResponseDto>
{
    #region Constructors
    public ProductListResponseDto(List<ProductBasicResponseDto> productResponseDtos, int pageCount, int itemCount)
    {
        Items = productResponseDtos;
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
