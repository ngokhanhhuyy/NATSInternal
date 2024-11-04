namespace NATSInternal.Services.Dtos;

public class OrderNewAuthorizationResponseDto : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}