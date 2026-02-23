using NATSInternal.Domain.Features.Products;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class BrandGetDetailResponseDto
{
    #region Constructors
    internal BrandGetDetailResponseDto(Brand brand)
    {
        Id = brand.Id;
        Name = brand.Name;
        Website = brand.Website;
        SocialMediaUrl = brand.SocialMediaUrl;
        PhoneNumber = brand.PhoneNumber;
        Email = brand.Email;
        Address = brand.Address;
        CreatedDateTime = brand.CreatedDateTime;
        
        if (brand.Country is not null)
        {
            Country = new(brand.Country);
        }
    }
    #endregion

    #region Properties 
    public Guid Id { get; }
    public string Name { get; }
    public string? Website { get; }
    public string? SocialMediaUrl { get; }
    public string? PhoneNumber { get; }
    public string? Email { get; }
    public string? Address { get; }
    public CountryBasicResponseDto? Country { get; }
    public DateTime CreatedDateTime { get; }
    #endregion
}