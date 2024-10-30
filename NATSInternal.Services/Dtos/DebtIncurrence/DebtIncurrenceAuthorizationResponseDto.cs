namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceAuthorizationResponseDto
    : IFinancialEngageableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetStatsDateTime { get; set; }
}