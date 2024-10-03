namespace NATSInternal.Services.Interfaces.Dtos;

public interface ILockableBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    DateTime StatsDateTime { get; internal set; }
    long Amount { get; internal set; }
    bool IsLocked { get; internal set; }
}