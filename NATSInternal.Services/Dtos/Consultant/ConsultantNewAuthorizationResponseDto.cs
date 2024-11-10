namespace NATSInternal.Services.Dtos;

public class ConsultantNewAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
