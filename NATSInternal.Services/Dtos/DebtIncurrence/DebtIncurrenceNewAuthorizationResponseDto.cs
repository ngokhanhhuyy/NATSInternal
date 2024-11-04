namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceNewAuthorizationResponseDto
    : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}