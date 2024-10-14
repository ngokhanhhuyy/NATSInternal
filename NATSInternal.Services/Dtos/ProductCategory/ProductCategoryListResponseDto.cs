namespace NATSInternal.Services.Dtos;

public class ProductCategoryListResponseDto : IListResponseDto<ProductCategoryResponseDto>
{
    public int PageCount { get; set; }
    public List<ProductCategoryResponseDto> Items { get; set; }
    public ProductCategoryAuthorizationResponseDto Authorization { get; set; }
}
