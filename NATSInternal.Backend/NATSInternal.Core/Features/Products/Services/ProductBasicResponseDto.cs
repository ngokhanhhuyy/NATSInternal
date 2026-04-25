using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Products;

public class ProductBasicResponseDto
{
    #region Properties
    public int Id { get; }
    public string Name { get; }
    public string Unit { get; }
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentagePerUnit { get; }
    public int StockingQuantity { get; }
    public bool IsResupplyNeeded { get; }
    public bool IsDiscontinued { get; }
    public string? ThumbnailUrl { get; }
    public List<ProductCategoryBasicResponseDto> Categories { get; }
    public ProductExistingAuthorizationResponseDto? Authorization { get; }
    #endregion

    #region Constructors
    internal ProductBasicResponseDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = product.DefaultVatPercentagePerUnit;
        IsDiscontinued = product.IsDiscontinued;
        Categories = product.Categories.Select(pc => new ProductCategoryBasicResponseDto(pc)).ToList();

        if (product.Stock is not null)
        {
            Stock stock = product.Stock;
            StockingQuantity = stock.StockingQuantity;
            IsResupplyNeeded = !product.IsDiscontinued && stock.StockingQuantity <= stock.ResupplyThresholdQuantity;
        }

        if (product.Thumbnail is not null)
        {
            ThumbnailUrl = product.Thumbnail.Url;
        }
    }
    
    internal ProductBasicResponseDto(
        Product product,
        ProductExistingAuthorizationResponseDto authorization) : this(product)
    {
        Authorization = authorization;
    }
    #endregion
}