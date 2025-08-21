namespace NATSInternal.Core.Dtos;

public class ProductUpsertRequestDto : IRequestDto
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
    public ProductCategoryUpsertRequestDto? Category { get; set; }
    public Guid? BrandId { get; set; }
    public List<PhotoRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Description = Description?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}