namespace NATSInternal.Services.Dtos;

public class BrandBasicResponseDto
    :
        IUpsertableBasicResponseDto<BrandAuthorizationResponseDto>,
        IHasThumbnailBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }
    public BrandAuthorizationResponseDto Authorization { get; set; }

    internal BrandBasicResponseDto(Brand brand)
    {
        MapFromEntity(brand);
    }

    internal BrandBasicResponseDto(Brand brand, BrandAuthorizationResponseDto authorization)
    {
        MapFromEntity(brand);
        Authorization = authorization;
    }

    private void MapFromEntity(Brand brand)
    {
        Id = brand.Id;
        Name = brand.Name;
        ThumbnailUrl = brand.ThumbnailUrl;
    }
}
