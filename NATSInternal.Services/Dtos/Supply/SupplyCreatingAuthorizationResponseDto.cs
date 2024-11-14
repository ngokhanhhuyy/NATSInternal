namespace NATSInternal.Services.Dtos;

public class SupplyCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}