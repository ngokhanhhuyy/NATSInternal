namespace NATSInternal.Core.Dtos;

public class ProductDetailResponseDto
        : IHasMultiplePhotosDetailResponseDto<ProductPhotoResponseDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Unit { get; set; }
    public long DefaultPrice { get; set; }
    public int DefaultVatPercentage { get; set; }
    public int StockingQuantity { get; set; }
    public bool IsForRetail { get; set; }
    public bool IsDiscontinued { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public string ThumbnailUrl { get; set; }
    public ProductCategoryResponseDto Category { get; set; }
    public BrandBasicResponseDto Brand { get; set; }
    public List<ProductPhotoResponseDto> Photos { get; set; }
    public ProductExistingAuthorizationResponseDto Authorization { get; set; }

    internal ProductDetailResponseDto(
            Product product,
            ProductExistingAuthorizationResponseDto authorization)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Unit = product.Unit;
        DefaultPrice = product.DefaultPrice;
        DefaultVatPercentage = product.DefaultVatPercentage;
        StockingQuantity = product.StockingQuantity;
        IsForRetail = product.IsForRetail;
        IsDiscontinued = product.IsDiscontinued;
        CreatedDateTime = product.CreatedDateTime;
        UpdatedDateTime = product.UpdatedDateTime;
        ThumbnailUrl = product.ThumbnailUrl;

        if (product.Category != null)
        {
            Category = new ProductCategoryResponseDto(product.Category);
        }

        if (product.Brand != null)
        {
            Brand = new BrandBasicResponseDto(product.Brand);
        }

        Authorization = authorization;
    }
}
