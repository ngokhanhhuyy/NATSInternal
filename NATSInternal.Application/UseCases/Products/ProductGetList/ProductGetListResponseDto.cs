using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetListResponseDto : IListResponseDto<ProductGetListProductResponseDto>
{
    #region Constructors
    public ProductGetListResponseDto(
        IEnumerable<ProductGetListProductResponseDto> productResponseDtos,
        int pageCount,
        int itemCount)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public IEnumerable<ProductGetListProductResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}

public class ProductGetListProductResponseDto
{
    #region Constructors
    internal ProductGetListProductResponseDto(
        Product product,
        Stock? stock,
        Photo? thumbnail,
        ProductExistingAuthorizationResponseDto authorization)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = product.DefaultVatPercentagePerUnit;

        if (product.Category is not null)
        {
            Category = new(product.Category);
        }

        if (product.Brand is not null)
        {
            Brand = new(product.Brand);
        }

        if (stock is not null)
        {
            StockingQuantity = stock.StockingQuantity;
            IsResupplyNeeded = !product.IsDiscontinued && stock.StockingQuantity <= stock.ResupplyThresholdQuantity;
        }

        if (thumbnail is not null)
        {
            ThumbnailUrl = thumbnail.Url;
        }

        Authorization = authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string Unit { get; }
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentagePerUnit { get; }
    public int StockingQuantity { get; }
    public bool IsResupplyNeeded { get; }
    public string? ThumbnailUrl { get; }
    public ProductCategoryBasicResponseDto? Category { get; }
    public BrandBasicResponseDto? Brand { get; }
    public ProductExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}