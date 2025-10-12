namespace NATSInternal.Domain.Features.Products;

public class ProductCategorySnapshot
{
    #region Constructors
    internal ProductCategorySnapshot(ProductCategory category)
    {
        Name = category.Name;
    }
    #endregion
    
    #region Properties
    public string Name { get; }
    #endregion
}