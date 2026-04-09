namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasStatsCreatingAuthorizationResponseDto
{
    bool CanSetStatsDateTime { get; set; }
}