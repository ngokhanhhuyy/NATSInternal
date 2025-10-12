namespace NATSInternal.Domain.Features.Products;

public class BrandSnapshot
{
    #region Constructors
    internal BrandSnapshot(Brand brand)
    {
        Name = brand.Name;
        Website = brand.Website;
        SocialMediaUrl = brand.SocialMediaUrl;
        PhoneNumber = brand.PhoneNumber;
        Email = brand.Email;
        Address = brand.Address;
        CreatedDateTime = brand.CreatedDateTime;
        CountryName = brand.Country?.Name;
    }
    #endregion
    
    #region Properties
    public string Name { get; }
    public string? Website { get; }
    public string? SocialMediaUrl { get; }
    public string? PhoneNumber { get; }
    public string? Email { get; }
    public string? Address { get; }
    public DateTime CreatedDateTime { get; }
    public string? CountryName { get; }
    #endregion
}