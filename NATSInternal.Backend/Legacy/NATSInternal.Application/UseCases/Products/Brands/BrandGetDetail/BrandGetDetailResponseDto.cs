using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Products;

public class BrandGetDetailResponseDto
{
    #region Constructors
    internal BrandGetDetailResponseDto(Brand brand, User? createdUser)
    {
        Id = brand.Id;
        Name = brand.Name;
        Website = brand.Website;
        SocialMediaUrl = brand.SocialMediaUrl;
        PhoneNumber = brand.PhoneNumber;
        Email = brand.Email;
        Address = brand.Address;
        CreatedUser = new(createdUser);
        CreatedDateTime = brand.CreatedDateTime;
        LastUpdatedDateTime = brand.LastUpdatedDateTime;

        if (brand.Country is not null)
        {
            Country = new(brand.Country);
        }
    }
    
    internal BrandGetDetailResponseDto(Brand brand, User? createdUser, User? lastUpdatedUser) : this(brand, createdUser)
    {
        LastUpdatedUser = new(lastUpdatedUser);
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
    public UserBasicResponseDto CreatedUser { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicResponseDto? LastUpdatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    #endregion
}