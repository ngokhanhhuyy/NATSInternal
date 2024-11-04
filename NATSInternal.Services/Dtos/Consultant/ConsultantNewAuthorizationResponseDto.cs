namespace NATSInternal.Services.Dtos;

public class ConsultantNewAuthorizationResponseDto
        : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
