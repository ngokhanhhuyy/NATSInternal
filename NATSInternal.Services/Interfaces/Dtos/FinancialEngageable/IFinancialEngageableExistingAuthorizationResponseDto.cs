namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableExistingAuthorizationResponseDto
    : IUpsertableExistingAuthorizationResponseDto
{
    bool CanSetStatsDateTime { get; set; }
}