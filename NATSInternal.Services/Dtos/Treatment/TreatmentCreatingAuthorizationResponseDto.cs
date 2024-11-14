namespace NATSInternal.Services.Dtos;

public class TreatmentCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}