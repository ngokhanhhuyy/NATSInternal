using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetDetailResponseDto
{
    #region Constructors
    internal ProductGetDetailResponseDto(
        Product product,
        User? createdUser,
        IEnumerable<Photo> photos,
        ProductExistingAuthorizationResponseDto authorizationResponseDto)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Unit = product.Unit;
        DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = product.DefaultVatPercentagePerUnit;
        IsForRetail = product.IsForRetail;
        IsDiscontinued = product.IsDiscontinued;
        CreatedDateTime = product.CreatedDateTime;
        CreatedUser = new(createdUser);
        LastUpdatedDateTime = product.LastUpdatedDateTime;
        Photos = photos.Select(p => new PhotoBasicResponseDto(p));
        Authorization = authorizationResponseDto;

        if (product.Brand is not null)
        {
            Brand = new(product.Brand);
        }

        if (product.Category is not null)
        {
            Category = new(product.Category);
        }
    }

    internal ProductGetDetailResponseDto(
        Product product,
        User? createdUser,
        User? lastUpdatedUser,
        IEnumerable<Photo> photos,
        ProductExistingAuthorizationResponseDto authorizationResponseDto) : this(
            product,
            createdUser,
            photos,
            authorizationResponseDto)
    {
        LastUpdatedUser = new(lastUpdatedUser);
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string? Description { get; }
    public string Unit { get; }
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentagePerUnit { get; }
    public bool IsForRetail { get; }
    public bool IsDiscontinued { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto CreatedUser { get;  }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public ProductCategoryBasicResponseDto? Category { get; }
    public BrandBasicResponseDto? Brand { get; }
    public IEnumerable<PhotoBasicResponseDto> Photos { get; }
    public ProductExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}