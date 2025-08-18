namespace NATSInternal.Core.Dtos;

public class SupplyUpdateHistoryResponseDto : IHasProductUpdateHistoryResponseDto<SupplyItemUpdateHistoryDataDto>
{
    #region Fields
    private readonly SupplyUpdateHistoryData _oldData;
    private readonly SupplyUpdateHistoryData _newData;
    #endregion
    
    #region Properties
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
    #endregion

    #region Constructors
    
    internal SupplyUpdateHistoryResponseDto(UpdateHistory updateHistory)
    {
        _oldData = updateHistory.GetOldData<SupplyUpdateHistoryData>();
        _newData = updateHistory.GetNewData<SupplyUpdateHistoryData>();

        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
    #endregion
}