namespace NATSInternal.Core.Dtos;

public class TreatmentExistingAuthorizationResponseDto
        : IHasStatsExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetStatsDateTime { get; set; }
}