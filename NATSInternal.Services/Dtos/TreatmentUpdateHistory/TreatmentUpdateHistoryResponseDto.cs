namespace NATSInternal.Services.Dtos;

public class TreatmentUpdateHistoryResponseDto : IUpdateHistoryResponseDto
{
    private readonly TreatmentUpdateHistoryDataDto _oldData;
    private readonly TreatmentUpdateHistoryDataDto _newData;

    public DateTime UpdatedDateTime { get; internal set; }
    public UserBasicResponseDto UpdatedUser { get; internal set; }
    public string UpdatedReason { get; internal set; }

    public DateTime OldPaidDateTime => _oldData.PaidDateTime;
    public DateTime NewPaidDateTime => _newData.PaidDateTime;

    public long OldServiceAmount => _oldData.ServiceAmount;
    public long NewServiceAmount => _newData.ServiceAmount;

    public decimal OldServiceVatFactor => _oldData.ServiceVatFactor;
    public decimal NewServiceVatFactor => _newData.ServiceVatFactor;

    public string OldNote => _oldData.Note;
    public string NewNote => _newData.Note;

    public TreatmentTherapistUpdateHistoryDataDto OldTherapist =>
        _oldData.Therapist;
    public TreatmentTherapistUpdateHistoryDataDto NewTherapist =>
        _newData.Therapist;

    public List<TreatmentItemUpdateHistoryDataDto> OldItems => _oldData.Items;
    public List<TreatmentItemUpdateHistoryDataDto> NewItems => _oldData.Items;

    internal TreatmentUpdateHistoryResponseDto(TreatmentUpdateHistory updateHistory)
    {
        _oldData = JsonSerializer
            .Deserialize<TreatmentUpdateHistoryDataDto>(updateHistory.OldData);
        _newData = JsonSerializer
            .Deserialize<TreatmentUpdateHistoryDataDto>(updateHistory.NewData);
        UpdatedDateTime = updateHistory.UpdatedDateTime;
        UpdatedUser = new UserBasicResponseDto(updateHistory.UpdatedUser);
        UpdatedReason = updateHistory.Reason;
    }
}