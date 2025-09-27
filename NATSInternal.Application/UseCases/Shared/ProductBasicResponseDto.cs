using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;

namespace NATSInternal.Application.UseCases.Shared;

public class ProductBasicResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    public int DefaultVatPercentage { get; set; }
    public int StockingQuantity { get; set; }
    public bool IsResupplyNeeded { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ProductExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal ProductBasicResponseDto(
        Product product,
        Stock stock,
        Photo? thumbnailPhoto)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentage = product.DefaultVatPercentage;
        StockingQuantity = stock.StockingQuantity;
        IsResupplyNeeded = stock.StockingQuantity <= stock.ResupplyThresholdQuantity;
        ThumbnailUrl = thumbnailPhoto?.Url;
    }

    internal ProductBasicResponseDto(
        Product product,
        Stock stock,
        Photo? thumbnailPhoto,
        ProductExistingAuthorizationResponseDto authorization) : this(product, stock, thumbnailPhoto)
    {
        Authorization = authorization;
    }
    #endregion
}