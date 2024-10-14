namespace NATSInternal.Services.Dtos;

public class ExpenseAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetPaidDateTime { get; set; }

    [JsonIgnore]
    public bool CanSetStatsDateTime
    {
        get => CanSetPaidDateTime;
        set => CanSetPaidDateTime = value;
    }
}