namespace NATSInternal.Core.Dtos;

public class DebtPaymentUpdateHistoryDataDto : IDebtUpdateHistoryDataDto
{
    public long Amount { get; set; }
    public string Note { get; set; }
    public DateTime StatsDateTime { get; set; }

    public DebtPaymentUpdateHistoryDataDto() { }
    
    internal DebtPaymentUpdateHistoryDataDto(DebtPayment debtPayment)
    {
        Amount = debtPayment.Amount;
        Note = debtPayment.Note;
        StatsDateTime = debtPayment.StatsDateTime;
    }
}