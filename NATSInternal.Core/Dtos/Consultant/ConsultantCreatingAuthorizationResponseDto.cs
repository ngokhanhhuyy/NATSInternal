namespace NATSInternal.Core.Dtos;

public class ConsultantCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
