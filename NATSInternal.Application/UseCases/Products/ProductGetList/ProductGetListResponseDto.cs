using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetListResponseDto : IPageableListResponseDto<ProductGetListProductResponseDto>
{
    #region Constructors
    public ProductGetListResponseDto(ICollection<ProductGetListProductResponseDto> productResponseDtos, int pageCount)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public ICollection<ProductGetListProductResponseDto> Items { get; }
    public int PageCount { get; }
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
        DefaultVatPercentage = product.DefaultVatPercentagePerUnit;

        if (stock is not null)
        {
            StockingQuantity = stock.StockingQuantity;
            IsResupplyNeeded = stock.StockingQuantity <= stock.ResupplyThresholdQuantity;
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
    public int DefaultVatPercentage { get; }
    public int StockingQuantity { get; }
    public bool IsResupplyNeeded { get; }
    public string? ThumbnailUrl { get; }
    public ProductExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}