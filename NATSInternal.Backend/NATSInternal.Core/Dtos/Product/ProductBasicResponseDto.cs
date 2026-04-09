namespace NATSInternal.Core.Dtos;

public class ProductBasicResponseDto : IUpsertableBasicResponseDto<ProductExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    public int DefaultVatPercentage { get; set; }
    public int StockingQuantity { get; set; }
    public string? ThumbnailUrl { get; set; }
    public AnnouncementExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal ProductBasicResponseDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentage = product.DefaultVatPercentage;
        StockingQuantity = product.StockingQuantity;
        ThumbnailUrl = product.Photos
            .Where(p => p.IsThumbnail)
            .Select(p => p.Url)
            .SingleOrDefault();
    }

    internal ProductBasicResponseDto(Product product, ProductExistingAuthorizationResponseDto authorization)
        : this(product)
    {
        Authorization = authorization;
    }
    #endregion
}
