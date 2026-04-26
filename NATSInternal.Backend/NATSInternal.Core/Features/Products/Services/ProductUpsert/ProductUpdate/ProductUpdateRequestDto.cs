namespace NATSInternal.Core.Features.Products;

public class ProductUpdateRequestDto : AbstractProductUpsertRequestDto
{
    #region Properties
    public bool IsDiscontinued { get; set; }
    #endregion
}