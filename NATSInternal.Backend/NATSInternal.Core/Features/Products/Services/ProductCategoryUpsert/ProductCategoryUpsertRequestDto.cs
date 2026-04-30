using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Products;

public class ProductCategoryUpsertRequestDto : IRequestDto
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}