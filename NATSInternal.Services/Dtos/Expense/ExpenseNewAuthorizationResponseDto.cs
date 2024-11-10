namespace NATSInternal.Services.Dtos;

public class ExpenseNewAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
