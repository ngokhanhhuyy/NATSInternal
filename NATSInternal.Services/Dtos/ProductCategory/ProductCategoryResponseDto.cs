namespace NATSInternal.Services.Dtos;

public class ProductCategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    internal ProductCategoryResponseDto(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }
}
