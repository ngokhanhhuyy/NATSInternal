namespace NATSInternal.Services.Dtos;

public class OrderCreatingAuthorizationResponseDto : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}