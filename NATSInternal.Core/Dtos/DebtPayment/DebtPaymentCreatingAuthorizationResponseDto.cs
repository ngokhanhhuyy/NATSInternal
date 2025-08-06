namespace NATSInternal.Core.Dtos;

public class DebtPaymentCreatingAuthorizationResponseDto
        : IHasStatsCreatingAuthorizationResponseDto
{
    public bool CanSetStatsDateTime { get; set; }
}