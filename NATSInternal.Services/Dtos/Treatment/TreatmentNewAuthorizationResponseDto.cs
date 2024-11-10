namespace NATSInternal.Services.Dtos;

public class TreatmentNewAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}