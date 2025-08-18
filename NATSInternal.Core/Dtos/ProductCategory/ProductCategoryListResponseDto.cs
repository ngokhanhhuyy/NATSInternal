namespace NATSInternal.Core.Dtos;

public class ProductCategoryListResponseDto : IPageableListResponseDto<ProductCategoryResponseDto>
{
    public int PageCount { get; set; }
    public List<ProductCategoryResponseDto> Items { get; set; }
}
