namespace NATSInternal.Core.Dtos;

public class ProductCategoryResponseDto : IBasicResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ProductCategoryExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal ProductCategoryResponseDto(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }

    internal ProductCategoryResponseDto(
            ProductCategory category,
            ProductCategoryExistingAuthorizationResponseDto authoriztion) : this(category)
    {
        Authorization = authoriztion;
    }
    #endregion
}
