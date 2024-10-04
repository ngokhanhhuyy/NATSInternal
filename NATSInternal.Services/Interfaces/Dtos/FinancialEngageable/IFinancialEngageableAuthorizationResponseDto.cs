namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableAuthorizationResponseDto
    : IUpsertableAuthorizationResponseDto
{
    bool CanSetStatsDateTime { get; internal set; }
}