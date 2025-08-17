namespace NATSInternal.Core.Dtos;

public class ProductBasicResponseDto
        : IUpsertableBasicResponseDto<ProductExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public long DefaultPrice { get; set; }
    public int DefaultVatPercentage { get; set; }
    public int StockingQuantity { get; set; }
    public string ThumbnailUrl { get; set; }
    public ProductExistingAuthorizationResponseDto Authorization { get; set; }

    internal ProductBasicResponseDto(Product product)
    {
        MapFromEntity(product);
    }

    internal ProductBasicResponseDto(
            Product product,
            ProductExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(product);
        Authorization = authorization;
    }

    private void MapFromEntity(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        DefaultPrice = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentage = product.DefaultVatPercentage;
        StockingQuantity = product.StockingQuantity;
        ThumbnailUrl = product.ThumbnailUrl;
    }
}
