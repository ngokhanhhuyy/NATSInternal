namespace NATSInternal.Services.Dtos;

public class ExpenseNewAuthorizationResponseDto
        : IFinancialEngageableNewAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}
