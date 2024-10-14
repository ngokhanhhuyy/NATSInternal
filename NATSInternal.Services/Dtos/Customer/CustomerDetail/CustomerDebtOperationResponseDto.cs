namespace NATSInternal.Services.Dtos;

public class CustomerDebtOperationResponseDto
{
    public DebtOperationType Operation { get; set; }
    public long Amount { get; set; }
    public DateTime OperatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerDebtOperationAuthorizationResponseDto Authorization { get; set; }
    
    internal CustomerDebtOperationResponseDto(
            DebtIncurrence debtIncurrence,
            IAuthorizationInternalService authorizationService)
    {
        Operation = DebtOperationType.DebtIncurrence;
        Amount = debtIncurrence.Amount;
        OperatedDateTime = debtIncurrence.StatsDateTime;
        IsLocked = debtIncurrence.IsLocked;
        
        DebtIncurrenceAuthorizationResponseDto authorization;
        authorization = authorizationService.GetDebtIncurrenceAuthorization(debtIncurrence);
        Authorization = new CustomerDebtOperationAuthorizationResponseDto(authorization);
    }
    
    internal CustomerDebtOperationResponseDto(
            DebtPayment debtPayment,
            IAuthorizationInternalService authorizationService)
    {
        Operation = DebtOperationType.DebtPayment;
        Amount = debtPayment.Amount;
        OperatedDateTime = debtPayment.StatsDateTime;
        IsLocked = debtPayment.IsLocked;
        
        DebtPaymentAuthorizationResponseDto authorization;
        authorization = authorizationService.GetDebtPaymentAuthorization(debtPayment);
        Authorization = new CustomerDebtOperationAuthorizationResponseDto(authorization);
    }
}