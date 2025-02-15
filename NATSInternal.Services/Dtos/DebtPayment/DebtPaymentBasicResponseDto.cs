namespace NATSInternal.Services.Dtos;

public class DebtPaymentBasicResponseDto
    : IHasCustomerBasicResponseDto<DebtPaymentExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    public bool IsLocked { get; set; }
    public CustomerBasicResponseDto Customer { get; set; }
    public DebtPaymentExistingAuthorizationResponseDto Authorization { get; set; }

    internal DebtPaymentBasicResponseDto(DebtPayment payment)
    {
        MapFromEntity(payment);
    }

    internal DebtPaymentBasicResponseDto(
            DebtPayment payment,
            DebtPaymentExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(payment);
        Authorization = authorization;
    }

    private void MapFromEntity(DebtPayment payment)
    {
        Id = payment.Id;
        Amount = payment.Amount;
        Note = payment.Note;
        StatsDateTime = payment.StatsDateTime;
        IsLocked = payment.IsLocked;
        Customer = new CustomerBasicResponseDto(payment.Customer);
    }
}