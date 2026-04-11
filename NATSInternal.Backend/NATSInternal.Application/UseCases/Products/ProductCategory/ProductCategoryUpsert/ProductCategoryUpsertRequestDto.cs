namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryUpsertRequestDto : IRequestDto
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    #endregion

    #region Methods
    public virtual void TransformValues()
    {
        Name ??= string.Empty;
    }
    #endregion
}