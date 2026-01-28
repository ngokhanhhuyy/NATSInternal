using NATSInternal.Application.Extensions;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public abstract class AbstractProductUpsertRequestDto : IRequestDto
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Unit { get; set; } = string.Empty;
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    public int DefaultVatPercentagePerUnit { get; set; }
    public bool IsForRetail { get; set; }
    public Guid? BrandId { get; set; }
    public string? CategoryName { get; set; }
    public ProductUpsertStockRequestDto Stock { get; set; } = new();
    public List<PhotoCreateOrUpdateRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Description = Description?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}

public class ProductUpsertStockRequestDto
{
    #region Properties
    public int StockingQuantity { get; set; }
    public int ResupplyThresholdQuantity { get; set; }
    #endregion
}