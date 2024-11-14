namespace NATSInternal.Services.Dtos;

public class ConsultantCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
