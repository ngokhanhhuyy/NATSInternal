namespace NATSInternal.Services.Dtos;

public class DebtPaymentBasicResponseDto
    : ICustomerEngageableBasicResponseDto<DebtPaymentAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long AmountBeforeVat { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public DebtPaymentAuthorizationResponseDto Authorization { get; set; }

    internal DebtPaymentBasicResponseDto(DebtPayment payment)
    {
        MapFromEntity(payment);
    }

    internal DebtPaymentBasicResponseDto(
            DebtPayment payment,
            DebtPaymentAuthorizationResponseDto authorization)
    {
        MapFromEntity(payment);
        Authorization = authorization;
    }

    private void MapFromEntity(DebtPayment payment)
    {
        Id = payment.Id;
        AmountBeforeVat = payment.Amount;
        Note = payment.Note;
        StatsDateTime = payment.StatsDateTime;
        IsLocked = payment.IsLocked;
        Customer = new CustomerBasicResponseDto(payment.Customer);
    }
}