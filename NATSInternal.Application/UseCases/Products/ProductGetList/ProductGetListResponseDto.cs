using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetListResponseDto : IPageableListResponseDto<ProductGetListProductResponseDto>
{
    #region Constructors
    public ProductGetListResponseDto(
        ICollection<ProductGetListProductResponseDto> productResponseDtos,
        int pageCount)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public ICollection<ProductGetListProductResponseDto> Items { get; set; }
    public int PageCount { get; set; }
    #endregion
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
        StockingQuantity = stockingQuantity;
        ThumbnailUrl = thumbnailUrl;
    }

    internal ProductGetListProductResponseDto(
        Product product,
        int stockingQuantity,
        string? thumbnailUrl,
        ProductExistingAuthorizationResponseDto authorization) : this(product, stockingQuantity, thumbnailUrl)
    {
        Authorization = authorization;
    }
    #endregion
}