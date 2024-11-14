namespace NATSInternal.Services.Dtos;

public class ExpenseCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
