namespace NATSInternal.Core.Dtos;

public class ProductCategoryUpsertRequestDto : IRequestDto
{
    #region Properties
    public required string Name { get; set; }
    #endregion

    public void TransformValues() { }
}