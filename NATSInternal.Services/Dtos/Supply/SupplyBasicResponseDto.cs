namespace NATSInternal.Services.Dtos;

public class SupplyBasicResponseDto
    : IHasStatsBasicResponseDto<SupplyExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public long Amount { get; set; }
    public bool IsLocked { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public string ThumbnailUrl { get; set; }
    public SupplyExistingAuthorizationResponseDto Authorization { get; set; }

    internal SupplyBasicResponseDto(Supply supply)
    {
        MapFromEntity(supply);
    }

    internal SupplyBasicResponseDto(
            Supply supply,
            SupplyExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(supply);
        Authorization = authorization;
    }

    private void MapFromEntity(Supply supply)
    {
        Id = supply.Id;
        StatsDateTime = supply.StatsDateTime;
        Amount = supply.Amount;
        IsLocked = supply.IsLocked;
        CreatedUser = new UserBasicResponseDto(supply.CreatedUser);
        ThumbnailUrl = supply.ThumbnailUrl;
    }
}
