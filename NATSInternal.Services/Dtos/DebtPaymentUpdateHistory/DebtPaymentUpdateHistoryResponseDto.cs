namespace NATSInternal.Services.Dtos;

public class DebtPaymentUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly DebtPaymentUpdateHistoryDataDto _oldData;
    private readonly DebtPaymentUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }
    
    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;
    
    public long OldAmount => _oldData.Amount;
    public long NewAmount => _newData.Amount;
    
    public string OldNote => _oldData.Note;
    public string NewNote => _oldData.Note;
    
    internal DebtPaymentUpdateHistoryResponseDto(DebtPaymentUpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<DebtPaymentUpdateHistoryDataDto>(updateHistory.OldData);
        _newData = JsonSerializer
            .Deserialize<DebtPaymentUpdateHistoryDataDto>(updateHistory.NewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}