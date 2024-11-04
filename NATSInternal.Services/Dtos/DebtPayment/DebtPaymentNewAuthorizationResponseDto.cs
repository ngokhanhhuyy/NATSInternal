namespace NATSInternal.Services.Dtos;

public class DebtPaymentNewAuthorizationResponseDto
        : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}