namespace NATSInternal.Services.Dtos;

public class SupplyAuthorizationResponseDto : IFinancialEngageableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetSuppliedDateTime { get; set; }

    public bool CanSetStatsDateTime
    {
        get => CanSetSuppliedDateTime;
        set => CanSetSuppliedDateTime = value;
    }
}
