namespace NATSInternal.Services.Dtos;

public class ProductCategoryResponseDto : IBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProductCategoryAuthorizationResponseDto Authorization { get; set; }

    internal ProductCategoryResponseDto(ProductCategory category)
    {
        MapFromEntity(category);
    }

    internal ProductCategoryResponseDto(
            ProductCategory category,
            ProductCategoryAuthorizationResponseDto authoriztion)
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
