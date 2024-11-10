namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasStatsExistingAuthorizationResponseDto
    : IUpsertableExistingAuthorizationResponseDto
{
    bool CanSetStatsDateTime { get; set; }
}