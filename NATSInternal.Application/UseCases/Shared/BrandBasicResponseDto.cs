using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Shared;

public class BrandBasicResponseDto
{
    #region Constructors
    internal BrandBasicResponseDto(Brand brand)
    {
        Id = brand.Id;
        Name = brand.Name;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    #endregion
}