namespace NATSInternal.Core.Dtos;

public class OrderUpdateHistoryResponseDto 
        : IProductExportableUpdateHistoryResponseDto<OrderItemUpdateHistoryDataDto>
{
    private readonly OrderUpdateHistoryDataDto _oldData;
    private readonly OrderUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }
    
    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;
    
    public string OldNote => _oldData.Note;
    public string NewNote => _newData.Note;
    
    public List<OrderItemUpdateHistoryDataDto> OldItems => _oldData.Items;
    public List<OrderItemUpdateHistoryDataDto> NewItems => _newData.Items;
    
    internal OrderUpdateHistoryResponseDto(UpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<OrderUpdateHistoryDataDto>(updateHistory.SerializedOldData);
        _newData = JsonSerializer
            .Deserialize<OrderUpdateHistoryDataDto>(updateHistory.SerializedNewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}