namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasStatsCreatingAuthorizationResponseDto
{
    bool CanSetStatsDateTime { get; set; }
}