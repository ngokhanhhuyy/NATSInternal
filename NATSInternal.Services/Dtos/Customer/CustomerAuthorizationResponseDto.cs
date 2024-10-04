namespace NATSInternal.Services.Dtos;

public class CustomerAuthorizationResponseDto
    : IUpsertableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanCreateDebt { get; set; }
    public bool CanCreateDebtPayment { get; set; }
}