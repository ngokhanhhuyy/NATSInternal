using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Shared;

public class CountryBasicResponseDto
{
    #region Properties
    public Guid Id { get; }
    public string Code { get; }
    public string Name { get; }
    #endregion

    #region Constructors
    internal CountryBasicResponseDto(Country country)
    {
        Id = country.Id;
        Code = country.Code;
        Name = country.Name;
    }
    #endregion
}
