namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly DebtIncurrenceUpdateHistoryDataDto _oldData;
    private readonly DebtIncurrenceUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }
    
    public DateTime OldIncurredDateTime => _oldData.StatsDateTime;
    public DateTime NewInCurredDateTime => _newData.StatsDateTime;
    
    public long OldAmount => _oldData.Amount;
    public long NewAmount => _newData.Amount;
    
    public string OldNote => _oldData.Note;
    public string NewNote => _newData.Note;
    
    internal DebtIncurrenceUpdateHistoryResponseDto(
            DebtIncurrenceUpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<DebtIncurrenceUpdateHistoryDataDto>(updateHistory.OldData);
        _newData = JsonSerializer
            .Deserialize<DebtIncurrenceUpdateHistoryDataDto>(updateHistory.NewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}