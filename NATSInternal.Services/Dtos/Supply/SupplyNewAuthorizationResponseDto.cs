namespace NATSInternal.Services.Dtos;

public class SupplyNewAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}