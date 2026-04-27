using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Products;

public class ProductCategoryDetailResponseDto
{
    #region Constructors
    internal ProductCategoryDetailResponseDto(
        ProductCategory category,
        ProductCategoryExistingAuthorizationResponseDto authorization)
    {
        Id = category.Id;
        Name = category.Name;
        Authorization = authorization;
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public ProductCategoryExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}