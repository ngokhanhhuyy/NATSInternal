using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Products;

public class ProductDetailResponseDto
{
    #region Constructors
    internal ProductDetailResponseDto(
        Product product,
        ProductExistingAuthorizationResponseDto authorizationResponseDto)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = product.DefaultVatPercentagePerUnit;
        StockingQuantity = product.StockingQuantity;
        ResupplyThresholdQuantity = product.ResupplyThresholdQuantity;
        IsForRetail = product.IsForRetail;
        IsDiscontinued = product.IsDiscontinued;
        CreatedDateTime = product.CreatedDateTime;
        CreatedUser = new(product.CreatedUser);
        LastUpdatedDateTime = product.LastUpdatedDateTime;
        Photos = product.Photos.Select(p => new PhotoBasicResponseDto(p));
        Authorization = authorizationResponseDto;

        if (product.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(product.LastUpdatedUser);
        }

        Categories = product.Categories.Select(pc => new ProductCategoryBasicResponseDto(pc));
    }
    #endregion
    
    #region Properties
    public int Id { get; }
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
    public UserBasicResponseDto CreatedUser { get;  }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public IEnumerable<ProductCategoryBasicResponseDto> Categories { get; }
    public IEnumerable<PhotoBasicResponseDto> Photos { get; }
    public ProductExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}
