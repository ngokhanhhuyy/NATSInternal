namespace NATSInternal.Core.Dtos;

public class ProductCategoryListResponseDto : IPageableListResponseDto<ProductCategoryResponseDto>
{
    #region Properties
    public List<ProductCategoryResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion

    #region Constructors
    internal ProductCategoryListResponseDto(ICollection<ProductCategoryResponseDto> basicResponseDto, int pageCount)
    {
        Items.AddRange(basicResponseDto);
        PageCount = pageCount;
    }
    #endregion
}
