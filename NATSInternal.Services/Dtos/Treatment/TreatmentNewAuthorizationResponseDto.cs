namespace NATSInternal.Services.Dtos;

public class TreatmentNewAuthorizationResponseDto
        : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}