namespace NATSInternal.Services.Dtos;

public class DebtPaymentNewAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}