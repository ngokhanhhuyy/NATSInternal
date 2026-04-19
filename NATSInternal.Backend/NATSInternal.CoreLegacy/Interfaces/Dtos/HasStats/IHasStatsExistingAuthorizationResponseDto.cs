namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasStatsExistingAuthorizationResponseDto
    : IUpsertableExistingAuthorizationResponseDto
{
    bool CanSetStatsDateTime { get; set; }
}