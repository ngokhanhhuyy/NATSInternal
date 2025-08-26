namespace NATSInternal.Core.Dtos;

public class BrandDetailResponseDto : IUpsertableDetailResponseDto<BrandExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Website { get; set; }
    public string? SocialMediaUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public string? ThumbnailUrl { get; set; }
    public CountryResponseDto? Country { get; set; }
    public BrandExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal BrandDetailResponseDto(
            Brand brand,
            BrandExistingAuthorizationResponseDto authorizationResponseDto)
    {
        Id = brand.Id;
        Name = brand.Name;
        Website = brand.Website;
        SocialMediaUrl = brand.SocialMediaUrl;
        PhoneNumber = brand.PhoneNumber;
        Email = brand.Email;
        Address = brand.Address;
        CreatedDateTime = brand.CreatedDateTime;
        ThumbnailUrl = brand.Photos
            .Where(p => p.IsThumbnail)
            .Select(p => p.Url)
            .SingleOrDefault();
        Authorization = authorizationResponseDto;

        if (brand.Country != null)
        {
            Country = new CountryResponseDto(brand.Country);
        }
    }
    #endregion
}
