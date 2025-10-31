using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetDetailResponseDto
{
    #region Constructors
    internal ProductGetDetailResponseDto(
        Product product,
        Stock stock,
        User createdUser,
        User? lastUpdatedUser,
        IEnumerable<Photo> photos,
        ProductExistingAuthorizationResponseDto authorizationResponseDto)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = product.DefaultVatPercentagePerUnit;
        StockingQuantity = stock.StockingQuantity;
        ResupplyThresholdQuantity = stock.ResupplyThresholdQuantity;
        IsForRetail = product.IsForRetail;
        IsDiscontinued = product.IsDiscontinued;
        CreatedDateTime = product.CreatedDateTime;
        LastUpdatedDateTime = product.LastUpdatedDateTime;
        CreatedUser = new(createdUser);
        LastUpdatedUser = lastUpdatedUser is not null ? new(lastUpdatedUser) : null;
        Category = product.Category is not null ? new(product.Category) : null;
        Brand = product.Brand is not null ? new(product.Brand) : null;
        Photos = photos.Select(p => new PhotoBasicResponseDto(p)).ToList();
        Authorization = authorizationResponseDto;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public string Unit { get; }
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentagePerUnit { get; }
    public int StockingQuantity { get; }
    public int? ResupplyThresholdQuantity { get; }
    public bool IsForRetail { get; }
    public bool IsDiscontinued { get; }
    public DateTime CreatedDateTime { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public ProductCategoryBasicResponseDto? Category { get; }
    public BrandBasicResponseDto? Brand { get; }
    public List<PhotoBasicResponseDto> Photos { get; }
    public ProductExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}