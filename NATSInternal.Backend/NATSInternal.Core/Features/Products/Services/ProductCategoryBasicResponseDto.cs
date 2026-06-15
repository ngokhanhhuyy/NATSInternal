using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Products;

public class ProductCategoryBasicResponseDto
{
    #region Constructors
    internal ProductCategoryBasicResponseDto(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }
    
    internal ProductCategoryBasicResponseDto(
        ProductCategory category,
        int productCount,
        ProductCategoryExistingAuthorizationResponseDto authorization) : this(category)
    {
        ProductCount = productCount;
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public int? ProductCount { get; }
    public ProductCategoryExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}
