using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.AuditLogs;

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
    public string? CountryName { get; }
    #endregion
}