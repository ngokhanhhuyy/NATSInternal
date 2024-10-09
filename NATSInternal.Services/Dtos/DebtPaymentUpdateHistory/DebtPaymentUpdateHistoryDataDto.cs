namespace NATSInternal.Services.Dtos;

public class DebtPaymentUpdateHistoryDataDto : IDebtUpdateHistoryDataDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime PaidDateTime { get; set; }

    public DateTime StatsDateTime
    {
        get => PaidDateTime;
        set => PaidDateTime = value;
    }
    
    internal DebtPaymentUpdateHistoryDataDto(DebtPayment payment)
    {
        Amount = payment.Amount;
        Note = payment.Note;
        PaidDateTime = payment.PaidDateTime;
    }
}