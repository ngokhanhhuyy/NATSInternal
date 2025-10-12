using NATSInternal.Application.Extensions;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public abstract class AbstractProductUpsertRequestDto : IRequestDto
{
    #region Properties
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Unit { get; set; }
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    public int DefaultVatPercentage { get; set; }
    public bool IsForRetail { get; set; }
    public bool IsDiscontinued { get; set; }
    public bool ThumbnailChanged { get; set; }
    public Guid? BrandId { get; set; }
    public string? CategoryName { get; set; }
    public List<PhotoAddOrUpdateRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Description = Description?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}