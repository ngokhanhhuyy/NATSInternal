namespace NATSInternal.Core.Dtos;

public class SupplyExistingAuthorizationResponseDto
        : IHasStatsExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetStatsDateTime { get; set; }
}
