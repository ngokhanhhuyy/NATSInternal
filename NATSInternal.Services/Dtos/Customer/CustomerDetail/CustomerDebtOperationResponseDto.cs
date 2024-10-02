namespace NATSInternal.Services.Dtos;

public class CustomerDebtOperationResponseDto
{
    public DebtOperationType Operation { get; set; }
    public long Amount { get; set; }
    public DateTime OperatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerDebtOperationAuthorizationResponseDto Authorization { get; set; }
    
    internal CustomerDebtOperationResponseDto(
            DebtIncurrence debt,
            IAuthorizationInternalService authorizationService)
    {
        Operation = DebtOperationType.DebtIncurrence;
        Amount = debt.Amount;
        OperatedDateTime = debt.IncurredDateTime;
        IsLocked = debt.IsLocked;
        
        DebtIncurrenceAuthorizationResponseDto authorization;
        authorization = authorizationService.GetDebtIncurrenceAuthorization(debt);
        Authorization = new CustomerDebtOperationAuthorizationResponseDto(authorization);
    }
    
    internal CustomerDebtOperationResponseDto(
            DebtPayment payment,
            IAuthorizationInternalService authorizationService)
    {
        Operation = DebtOperationType.DebtPayment;
        Amount = payment.Amount;
        OperatedDateTime = payment.PaidDateTime;
        IsLocked = payment.IsLocked;
        
        DebtPaymentAuthorizationResponseDto authorization;
        authorization = authorizationService.GetDebtPaymentAuthorization(payment);
        Authorization = new CustomerDebtOperationAuthorizationResponseDto(authorization);
    }
}