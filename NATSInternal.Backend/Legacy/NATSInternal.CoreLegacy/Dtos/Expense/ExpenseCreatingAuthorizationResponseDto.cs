namespace NATSInternal.Core.Dtos;

public class ExpenseCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
