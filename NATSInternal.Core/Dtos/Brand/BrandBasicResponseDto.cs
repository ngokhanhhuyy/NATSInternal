namespace NATSInternal.Core.Dtos;

public class BrandBasicResponseDto
    :
        IUpsertableBasicResponseDto<BrandExistingAuthorizationResponseDto>,
        IHasThumbnailBasicResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? ThumbnailUrl { get; set; }
    public BrandExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal BrandBasicResponseDto(Brand brand)
    {
        Id = brand.Id;
        Name = brand.Name;
        ThumbnailUrl = brand.Photos
            .Where(p => p.IsThumbnail)
            .Select(p => p.Url)
            .SingleOrDefault();
    }

    internal BrandBasicResponseDto(Brand brand, BrandExistingAuthorizationResponseDto authorization) : this(brand)
    {
        Authorization = authorization;
    }
    #endregion
}
