namespace NATSInternal.Services.Dtos;

public class OrderNewAuthorizationResponseDto : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}