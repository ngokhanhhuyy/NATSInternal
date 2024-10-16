namespace NATSInternal.Services.Dtos;

public class ConsultantUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly ConsultantUpdateHistoryDataDto _oldData;
    private readonly ConsultantUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }
    
    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;
    
    public long OldAmount => _oldData.Amount;
    public long NewAmount => _newData.Amount;
    
    public string OldNote => _oldData.Note;
    public string NewNote => _newData.Note;
    
    internal ConsultantUpdateHistoryResponseDto(ConsultantUpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<ConsultantUpdateHistoryDataDto>(updateHistory.OldData);
        _newData = JsonSerializer
            .Deserialize<ConsultantUpdateHistoryDataDto>(updateHistory.NewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}