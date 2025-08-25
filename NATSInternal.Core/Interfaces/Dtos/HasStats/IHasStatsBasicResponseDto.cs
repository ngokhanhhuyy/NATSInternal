namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasStatsBasicResponseDto<out TExistingAuthorization>
    : IUpsertableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IHasStatsExistingAuthorizationResponseDto
{
    #region Properties
    DateTime StatsDateTime { get; }
    long Amount { get; }
    bool IsLocked { get; }
    #endregion
}