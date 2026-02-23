namespace NATSInternal.Application.UseCases.Products;

public class BrandUpsertRequestDto : IRequestDto
{
    #region Properties
    public string Name { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? SocialMediaUrl { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public Guid? CountryId { get; set; }
    #endregion

    #region Methods
    public virtual void TransformValues()
    {
        Name ??= string.Empty;
    }
    #endregion
}