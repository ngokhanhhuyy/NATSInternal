namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasStatsBasicResponseDto<TExistingAuthorization>
    : IUpsertableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    DateTime StatsDateTime { get; }
    long Amount { get; }
    bool IsLocked { get; }
}