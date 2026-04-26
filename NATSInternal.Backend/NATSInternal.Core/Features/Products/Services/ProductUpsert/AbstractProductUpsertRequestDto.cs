using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Products;

public abstract class AbstractProductUpsertRequestDto : IRequestDto
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Unit { get; set; } = string.Empty;
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    public int DefaultVatPercentagePerUnit { get; set; }
    public bool IsForRetail { get; set; }
    public List<int> CategoryIds { get; set; } = new();
    public int? ResupplyThresholdQuantity { get; set; }
    public List<PhotoCreateOrUpdateRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Description = Description?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}