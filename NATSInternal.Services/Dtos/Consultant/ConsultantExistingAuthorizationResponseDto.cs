namespace NATSInternal.Services.Dtos;

public class ConsultantExistingAuthorizationResponseDto
    : IHasStatsExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetStatsDateTime { get; set; }
    public bool CanAccessUpdateHistories { get; set; }
}