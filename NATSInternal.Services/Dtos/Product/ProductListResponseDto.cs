namespace NATSInternal.Services.Dtos;

public class ProductListResponseDto
{
    public int PageCount { get; set; }
    public List<ProductBasicResponseDto> Items { get; set; }
    public ProductListAuthorizationResponseDto Authorization { get; set; }
}
