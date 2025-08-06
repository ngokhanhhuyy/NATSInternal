namespace NATSInternal.Core.Dtos;

public class DebtIncurrenceUpdateHistoryResponseDto : IDebtUpdateHistoryResponseDto
{
    private readonly DebtIncurrenceUpdateHistoryDataDto _oldData;
    private readonly DebtIncurrenceUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }
    
    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;
    
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