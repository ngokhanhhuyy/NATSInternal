namespace NATSInternal.Core.Features.Products;

public class ProductUpdateRequestDto : AbstractProductUpsertRequestDto
{
    #region Properties
    public Guid Id { get; set; }
    public bool IsDiscontinued { get; set; }
    #endregion
}