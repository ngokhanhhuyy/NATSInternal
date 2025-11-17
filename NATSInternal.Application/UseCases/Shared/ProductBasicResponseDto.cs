using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Shared;

public class ProductBasicResponseDto
{
    #region Properties
    public Guid Id { get; } = Guid.Empty;
    public string Name { get; } = string.Empty;
    public string Unit { get; } = string.Empty;
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentage { get; }
    public bool IsResupplyNeeded { get; }
    public ProductExistingAuthorizationResponseDto? Authorization { get; }
    public bool IsDeleted { get; }
    #endregion

    #region Constructors
    internal ProductBasicResponseDto(Product? product)
    {
        if (product is not null && product.DeletedDateTime is null)
        {
            Id = product.Id;
            Name = product.Name;
            Unit = product.Unit;
            DefaultAmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit;
            DefaultVatPercentage = product.DefaultVatPercentagePerUnit;
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