namespace NATSInternal.Services.Dtos;

public class SupplyNewAuthorizationResponseDto
        : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}