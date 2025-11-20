using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

public class ProductGetDetailResponseDto
{
    #region Constructors
    internal ProductGetDetailResponseDto(
        Product product,
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
        LastUpdatedDateTime = product.LastUpdatedDateTime;
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
    public DateTime? LastUpdatedDateTime { get; }
    public ProductCategoryBasicResponseDto? Category { get; }
    public BrandBasicResponseDto? Brand { get; }
    public ProductExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}