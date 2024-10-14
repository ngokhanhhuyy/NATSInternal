namespace NATSInternal.Services.Dtos;

public class ConsultantUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly ConsultantUpdateHistoryDataDto _oldData;
    private readonly ConsultantUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; private set; }
    public UserBasicResponseDto UpdatedUser { get; private set; }
    public string UpdatedReason { get; private set; }
    
    public DateTime OldPaidDateTime => _oldData.StatsDateTime;
    public DateTime NewPaidDateTime => _newData.StatsDateTime;
    
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