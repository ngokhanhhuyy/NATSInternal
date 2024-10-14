namespace NATSInternal.Services.Dtos;

public class SupplyBasicResponseDto
    : IFinancialEngageableBasicResponseDto<SupplyAuthorizationResponseDto>
{
    public int Id { get; set; }
    public DateTime StatsDateTime { get; set; }
    public long Amount { get; set; }
    public bool IsLocked { get; set; }
    public UserBasicResponseDto User { get; set; }
    public string FirstPhotoUrl { get; set; }
    public SupplyAuthorizationResponseDto Authorization { get; set; }

    internal SupplyBasicResponseDto(Supply supply)
    {
        MapFromEntity(supply);
    }

    internal SupplyBasicResponseDto(
            Supply supply,
            SupplyAuthorizationResponseDto authorization)
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
        User = new UserBasicResponseDto(supply.CreatedUser);
        FirstPhotoUrl = supply.FirstPhotoUrl;
    }
}
