using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Shared;

public class ProductBasicResponseDto
{
    #region Properties
    public Guid Id { get; } = Guid.Empty;
    public string Name { get; } = string.Empty;
    public string Unit { get; } = string.Empty;
    public bool IsDeleted { get; } = true;
    #endregion

    #region Constructors
    internal ProductBasicResponseDto(Product? product)
    {
        if (product is not null && product.DeletedDateTime is null)
        {
            Id = product.Id;
            Name = product.Name;
            Unit = product.Unit;
        }
    }
    #endregion
}