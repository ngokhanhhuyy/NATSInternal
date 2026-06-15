using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Products;

public class ProductCategoryDetailResponseDto
{
    #region Constructors
    internal ProductCategoryDetailResponseDto(
        ProductCategory category,
        int productCount,
        ProductCategoryExistingAuthorizationResponseDto authorization)
    {
        Id = category.Id;
        Name = category.Name;
        ProductCount = productCount;
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public int ProductCount { get; }
    public ProductCategoryExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}
