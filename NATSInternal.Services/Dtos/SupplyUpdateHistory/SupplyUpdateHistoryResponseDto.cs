namespace NATSInternal.Services.Dtos;

public class SupplyUpdateHistoryResponseDto
        : IHasProductUpdateHistoryResponseDto<SupplyItemUpdateHistoryDataDto>
{
    private readonly SupplyUpdateHistoryDataDto _oldData;
    private readonly SupplyUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }
    
    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;
    
    public long OldShipmentFee => _oldData.ShipmentFee;
    public long NewShipementFee => _newData.ShipmentFee;
    
    public string OldNote => _oldData.Note;
    public string NewNote => _newData.Note;
    
    public List<SupplyItemUpdateHistoryDataDto> OldItems => _oldData.Items;
    public List<SupplyItemUpdateHistoryDataDto> NewItems => _newData.Items;
    
    internal SupplyUpdateHistoryResponseDto(SupplyUpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<SupplyUpdateHistoryDataDto>(updateHistory.OldData);
        _newData = JsonSerializer
            .Deserialize<SupplyUpdateHistoryDataDto>(updateHistory.NewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}