namespace NATSInternal.Services.Dtos;

public class CustomerDebtOperationAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    
    public CustomerDebtOperationAuthorizationResponseDto(
            DebtIncurrenceExistingAuthorizationResponseDto authorization)
    {
        CanEdit = authorization.CanEdit;
        CanDelete = authorization.CanDelete;
    }
    
    public CustomerDebtOperationAuthorizationResponseDto(
            DebtPaymentExistingAuthorizationResponseDto authorization)
    {
        CanEdit = authorization.CanEdit;
        CanDelete = authorization.CanDelete;
    }
}