using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetListResponseDto : IPageableListResponseDto<ProductBasicResponseDto>
{
    #region Constructors
    public ProductGetListResponseDto(ICollection<ProductBasicResponseDto> productResponseDtos, int pageCount)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public ICollection<ProductBasicResponseDto> Items { get; }
    public int PageCount { get; }
    #endregion
}