namespace NATSInternal.Core.Dtos;

public class ProductListResponseDto
    : IUpsertableListResponseDto<ProductBasicResponseDto, ProductExistingAuthorizationResponseDto>
{
    #region Constructors
    internal ProductListResponseDto(ICollection<Product> products, int pageCount)
    {
        Items.AddRange(products.Select(p => new ProductBasicResponseDto(p)));
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public List<ProductBasicResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion
}
