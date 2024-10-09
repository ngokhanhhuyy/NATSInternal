namespace NATSInternal.Services.Dtos;

public class DebtPaymentUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly DebtPaymentUpdateHistoryDataDto _oldData;
    private readonly DebtPaymentUpdateHistoryDataDto _newData;
    
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string Reason { get; set; }
    
    public DateTime OldPaidDateTime => _oldData.PaidDateTime;
    public DateTime NewPaidDateTime => _newData.PaidDateTime;
    
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
        Reason = updateHistory.Reason;
    }
}