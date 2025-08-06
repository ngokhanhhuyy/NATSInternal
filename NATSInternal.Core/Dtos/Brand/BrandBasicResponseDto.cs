namespace NATSInternal.Core.Dtos;

public class BrandBasicResponseDto
    :
        IUpsertableBasicResponseDto<BrandExistingAuthorizationResponseDto>,
        IHasThumbnailBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ThumbnailUrl { get; set; }
    public BrandExistingAuthorizationResponseDto Authorization { get; set; }

    internal BrandBasicResponseDto(Brand brand)
    {
        MapFromEntity(brand);
    }

    internal BrandBasicResponseDto(
            Brand brand,
            BrandExistingAuthorizationResponseDto authorization)
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
