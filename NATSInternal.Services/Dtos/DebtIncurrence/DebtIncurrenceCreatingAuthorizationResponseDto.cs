namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceCreatingAuthorizationResponseDto
    : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}