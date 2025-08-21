namespace NATSInternal.Core.Dtos;

public class BrandUpsertRequestDto : IHasPhotosUpsertRequestDto
{
    #region Properties
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Website { get; set; }
    public required string? SocialMediaUrl { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? Email { get; set; }
    public required string? Address { get; set; }
    public Guid? CountryId { get; set; }
    public List<PhotoRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Website = Website?.ToNullIfEmptyOrWhiteSpace();
        SocialMediaUrl = SocialMediaUrl?.ToNullIfEmptyOrWhiteSpace();
        PhoneNumber = PhoneNumber?.ToNullIfEmptyOrWhiteSpace();
        Email = Email?.ToNullIfEmptyOrWhiteSpace();
        Address = Address?.ToNullIfEmptyOrWhiteSpace();

        if (CountryId == Guid.Empty)
        {
            CountryId = null;
        }
    }
    #endregion
}
