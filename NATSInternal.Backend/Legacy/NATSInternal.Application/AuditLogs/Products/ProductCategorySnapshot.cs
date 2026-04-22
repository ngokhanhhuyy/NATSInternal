using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.AuditLogs;

public class ProductCategorySnapshot
{
    #region Constructors
    internal ProductCategorySnapshot(ProductCategory productCategory)
    {
        Name = productCategory.Name;
    }
    #endregion

    #region Properties
    public string Name { get; }
    #endregion
}