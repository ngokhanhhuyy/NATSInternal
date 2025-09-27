using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Shared;

public class ProductCategoryBasicResponseDto
{
    #region Constructors
    internal ProductCategoryBasicResponseDto(ProductCategory category)
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