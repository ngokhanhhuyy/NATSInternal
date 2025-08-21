namespace NATSInternal.Core.Dtos;

public class ProductCategoryListResponseDto : IPageableListResponseDto<ProductCategoryResponseDto>
{
    #region Properties
    public List<ProductCategoryResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion

    #region Constructors
    internal ProductCategoryListResponseDto(ICollection<ProductCategory> productCategories, int pageCount)
    {
        Items.AddRange(productCategories.Select(pc => new ProductCategoryResponseDto(pc)));
        PageCount = pageCount;
    }
    #endregion
}
