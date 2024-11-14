namespace NATSInternal.Services.Dtos;

public class ProductCategoryMinimalResponseDto : IMinimalResponseDto
{
    public int Id { get; internal set; }
    public string Name { get; internal set; }

    internal ProductCategoryMinimalResponseDto(ProductCategory category)
    {
        Id = category.Id;
        Name = category.Name;
    }
}
