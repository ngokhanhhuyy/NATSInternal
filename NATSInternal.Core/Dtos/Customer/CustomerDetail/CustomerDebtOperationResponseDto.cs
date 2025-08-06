namespace NATSInternal.Core.Dtos;

public class CustomerDebtOperationResponseDto
{
    public int Id { get; set; }
    public DebtOperationType Operation { get; set; }
    public long Amount { get; set; }
    public DateTime OperatedDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerDebtOperationAuthorizationResponseDto Authorization { get; set; }
    
    internal CustomerDebtOperationResponseDto(
            DebtIncurrence debtIncurrence,
            DebtIncurrenceExistingAuthorizationResponseDto authorization)
    {
        Id = debtIncurrence.Id;
        Operation = DebtOperationType.DebtIncurrence;
        Amount = debtIncurrence.Amount;
        OperatedDateTime = debtIncurrence.StatsDateTime;
        IsLocked = debtIncurrence.IsLocked;
        Authorization = new CustomerDebtOperationAuthorizationResponseDto(authorization);
    }
    
    internal CustomerDebtOperationResponseDto(
            DebtPayment debtPayment,
            DebtPaymentExistingAuthorizationResponseDto authorization)
    {
        Id = debtPayment.Id;
        Operation = DebtOperationType.DebtPayment;
        Amount = debtPayment.Amount;
        OperatedDateTime = debtPayment.StatsDateTime;
        IsLocked = debtPayment.IsLocked;
        Authorization = new CustomerDebtOperationAuthorizationResponseDto(authorization);
    }
}