namespace NATSInternal.Core.Dtos;

public class DebtUpdateHistoryResponseDto : IDebtUpdateHistoryResponseDto
{
    #region Fields
    private readonly DebtUpdateHistoryData _oldData;
    private readonly DebtUpdateHistoryData _newData;
    #endregion

    #region Properties
    public DateTime UpdatedDateTime { get; set; }
    public UserBasicResponseDto UpdatedUser { get; set; }
    public string UpdatedReason { get; set; }

    public DateTime OldStatsDateTime => _oldData.StatsDateTime;
    public DateTime NewStatsDateTime => _newData.StatsDateTime;

    public long OldAmount => _oldData.Amount;
    public long NewAmount => _newData.Amount;

    public string? OldNote => _oldData.Note;
    public string? NewNote => _newData.Note;
    #endregion

    #region Constructors
    internal DebtUpdateHistoryResponseDto(UpdateHistory updateHistory)
    {
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;

        _oldData = updateHistory.GetOldData<DebtUpdateHistoryData>();
        _newData = updateHistory.GetNewData<DebtUpdateHistoryData>();
    }
    #endregion
}