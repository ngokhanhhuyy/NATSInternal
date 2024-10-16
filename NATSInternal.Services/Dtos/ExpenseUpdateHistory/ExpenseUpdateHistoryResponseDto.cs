namespace NATSInternal.Services.Dtos;

public class ExpenseUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly ExpenseUpdateHistoryDataDto _oldData;
    private readonly ExpenseUpdateHistoryDataDto _newData;

    public DateTime UpdatedDateTime { get; }
    public UserBasicResponseDto UpdatedUser { get; }
    public string UpdatedReason { get; }

    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;
    
    public long OldAmount => _oldData.Amount;
    public long NewAmount => _newData.Amount;

    public ExpenseCategory OldCategory => _oldData.Category;
    public ExpenseCategory NewCategory => _newData.Category;

    public string OldNote => _oldData.Note;
    public string NewNote => _newData.Note;

    public string OldPayeeName => _oldData.PayeeName;
    public string NewPayeeName => _newData.PayeeName;

    internal ExpenseUpdateHistoryResponseDto(ExpenseUpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<ExpenseUpdateHistoryDataDto>(updateHistory.OldData);
        _newData = JsonSerializer
            .Deserialize<ExpenseUpdateHistoryDataDto>(updateHistory.NewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}