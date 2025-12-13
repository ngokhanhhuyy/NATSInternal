using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryGetListResponseDto
    : IPageableListResponseDto<ProductCategoryGetListProductCategoryResponseDto>
{
    #region Constructors
    public ProductCategoryGetListResponseDto(
        IEnumerable<ProductCategoryGetListProductCategoryResponseDto> productResponseDtos,
        int pageCount,
        int itemCount)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public IEnumerable<ProductCategoryGetListProductCategoryResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}

public class ProductCategoryGetListProductCategoryResponseDto
{
    #region Constructors
    internal ProductCategoryGetListProductCategoryResponseDto(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    #endregion
}