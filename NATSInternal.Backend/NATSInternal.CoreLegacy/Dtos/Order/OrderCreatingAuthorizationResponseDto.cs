namespace NATSInternal.Core.Dtos;

public class OrderCreatingAuthorizationResponseDto : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}