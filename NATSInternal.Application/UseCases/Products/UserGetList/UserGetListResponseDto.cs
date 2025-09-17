using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetListResponseDto
{
    
}

public class ProductGetListProductResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    public int DefaultVatPercentage { get; set; }
    public int StockingQuantity { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ProductExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal ProductGetListProductResponseDto(Product product, int stockingQuantity, string? thumbnailUrl)
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

    internal ProductGetListProductResponseDto(Product product, ProductExistingAuthorizationResponseDto authorization)
        : this(product)
    {
        Authorization = authorization;
    }
}