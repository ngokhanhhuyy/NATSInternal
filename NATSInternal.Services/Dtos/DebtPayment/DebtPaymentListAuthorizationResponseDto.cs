namespace NATSInternal.Services.Dtos;

public class DebtPaymentListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}