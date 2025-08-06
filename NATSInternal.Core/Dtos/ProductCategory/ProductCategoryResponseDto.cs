namespace NATSInternal.Core.Dtos;

public class ProductCategoryResponseDto : IBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductCategoryExistingAuthorizationResponseDto Authorization { get; set; }

    internal ProductCategoryResponseDto(ProductCategory category)
    {
        MapFromEntity(category);
    }

    internal ProductCategoryResponseDto(
            ProductCategory category,
            ProductCategoryExistingAuthorizationResponseDto authoriztion)
    {
        MapFromEntity(category);
        Authorization = authoriztion;
    }

    private void MapFromEntity(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }
}
