namespace NATSInternal.Services.Dtos;

public class TreatmentAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetPaidDateTime { get; set; }

    public bool CanSetStatsDateTime
    {
        get => CanSetPaidDateTime;
        set => CanSetPaidDateTime = value;
    }
}