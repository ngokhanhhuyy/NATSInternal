namespace NATSInternal.Services.Dtos;

public class DebtPaymentExistingAuthorizationResponseDto
    : IFinancialEngageableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetStatsDateTime { get; set; }
}
