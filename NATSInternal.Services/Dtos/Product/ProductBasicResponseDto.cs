namespace NATSInternal.Services.Dtos;

public class ProductBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public long Price { get; set; }
    public int StockingQuantity { get; set; }
    public string ThumbnailUrl { get; set; }
    public ProductAuthorizationResponseDto Authorization { get; set; }

    internal ProductBasicResponseDto(Product product)
    {
        MapFromEntity(product);
    }

    internal ProductBasicResponseDto(
            Product product,
            ProductAuthorizationResponseDto authorization)
    {
        MapFromEntity(product);
        Authorization = authorization;
    }

    private void MapFromEntity(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        Price = product.DefaultPrice;
        StockingQuantity = product.StockingQuantity;
        ThumbnailUrl = product.ThumbnailUrl;
    }
}
