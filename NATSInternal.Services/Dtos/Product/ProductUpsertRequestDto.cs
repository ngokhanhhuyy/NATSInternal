namespace NATSInternal.Services.Dtos;

public class ProductUpsertRequestDto
        : IRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Unit { get; set; }
    public long DefaultPrice { get; set; }
    public int DefaultVatPercentage { get; set; }
    public bool IsForRetail { get; set; }
    public bool IsDiscontinued { get; set; }
    public byte[] ThumbnailFile { get; set; }
    public bool ThumbnailChanged { get; set; }
    public int CategoryId { get; set; }
    public int BrandId { get; set; }
    public List<ProductPhotoRequestDto> Photos { get; set; }

    public void TransformValues()
    {
        Name = Name?.ToNullIfEmpty();
        Description = Description?.ToNullIfEmpty();
        Unit = Unit?.ToNullIfEmpty();
    }
}