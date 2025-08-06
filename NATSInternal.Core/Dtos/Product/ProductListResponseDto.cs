namespace NATSInternal.Core.Dtos;

public class ProductListResponseDto : IUpsertableListResponseDto<
        ProductBasicResponseDto, ProductExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<ProductBasicResponseDto> Items { get; set; }
}
