namespace NATSInternal.Services.Dtos;

public class DebtPaymentCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}