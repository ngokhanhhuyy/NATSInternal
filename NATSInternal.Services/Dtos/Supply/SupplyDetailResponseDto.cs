namespace NATSInternal.Services.Dtos;

public class SupplyDetailResponseDto : IProductEngageableDetailResponseDto<
        SupplyItemResponseDto,
        SupplyPhotoResponseDto,
        SupplyUpdateHistoryResponseDto, 
        SupplyItemUpdateHistoryDataDto,
        SupplyExistingAuthorizationResponseDto>
{
    public int Id { get; internal set; }
    public DateTime StatsDateTime { get; internal set; }
    public long ShipmentFee { get; internal set; }
    public string Note { get; internal set; }
    public DateTime CreatedDateTime { get; internal set; }
    public UserBasicResponseDto CreatedUser { get; internal set; }
    public DateTime? UpdatedDateTime { get; internal set; }
    public bool IsLocked { get; internal set; }
    public List<SupplyItemResponseDto> Items { get; internal set; }
    public List<SupplyPhotoResponseDto> Photos { get; internal set; }
    public SupplyExistingAuthorizationResponseDto Authorization { get; internal set; }
    public List<SupplyUpdateHistoryResponseDto> UpdateHistories { get; internal set; }

    [JsonIgnore]
    public long AmountAfterVat => Items.Sum(i => i.ProductAmountPerUnit * i.Quantity);

    public string ThumbnailUrl => Photos?
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    internal SupplyDetailResponseDto(
            Supply supply,
            SupplyExistingAuthorizationResponseDto authorization)
    {
        Id = supply.Id;
        StatsDateTime = supply.StatsDateTime;
        ShipmentFee = supply.ShipmentFee;
        Note = supply.Note;
        IsLocked = supply.IsLocked;
        CreatedDateTime = supply.CreatedDateTime;
        UpdatedDateTime = supply.UpdatedDateTime;
        Items = supply.Items?
            .OrderBy(i => i.Id)
            .Select(i => new SupplyItemResponseDto(i)).ToList();
        Photos = supply.Photos?
            .OrderBy(p => p.Id)
            .Select(p => new SupplyPhotoResponseDto(p)).ToList();
        CreatedUser = new UserBasicResponseDto(supply.CreatedUser);
        Authorization = authorization;
        UpdateHistories = supply.UpdateHistories
            .Select(uh => new SupplyUpdateHistoryResponseDto(uh))
            .ToList();
    }
}
