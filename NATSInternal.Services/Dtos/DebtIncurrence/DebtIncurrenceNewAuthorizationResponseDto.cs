namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceNewAuthorizationResponseDto
    : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}