namespace NATSInternal.Core.Dtos;

public class LastestTransactionResponseDto
{
    public int Id { get; set; }
    public TransactionDirection Direction { get; set; }
    public TransactionType Type { get; set; }
    public long Amount { get; set; }
    public DateTime StatsDateTime { get; set; }

    internal LastestTransactionResponseDto(Supply supply)
    {
        Id = supply.Id;
        Direction = TransactionDirection.Out;
        Type = TransactionType.Supply;
        Amount = supply.Amount;
        StatsDateTime = supply.StatsDateTime;
    }

    internal LastestTransactionResponseDto(Expense expense)
    {
        Id = expense.Id;
        Direction = TransactionDirection.Out;
        Type = TransactionType.Expense;
        Amount = expense.Amount;
        StatsDateTime = expense.StatsDateTime;
    }

    internal LastestTransactionResponseDto(Consultant consultant)
    {
        Id = consultant.Id;
        Direction = TransactionDirection.In;
        Type = TransactionType.Consultant;
        Amount = consultant.AmountBeforeVat + consultant.VatAmount;
        StatsDateTime = consultant.StatsDateTime;
    }

    internal LastestTransactionResponseDto(Order order)
    {
        Id = order.Id;
        Direction = TransactionDirection.In;
        Type = TransactionType.Retail;
        Amount = order.AmountAfterVat;
        StatsDateTime = order.StatsDateTime;
    }

    internal LastestTransactionResponseDto(Treatment treatment)
    {
        Id = treatment.Id;
        Direction = TransactionDirection.In;
        Type = TransactionType.Treatment;
        Amount = treatment.AmountAfterVat;
        StatsDateTime = treatment.StatsDateTime;
    }

    internal LastestTransactionResponseDto(Debt debtIncurrence)
    {
        Id = debtIncurrence.Id;
        Direction = TransactionDirection.In;
        Type = TransactionType.DebtIncurrence;
        Amount = debtIncurrence.Amount;
        StatsDateTime = debtIncurrence.StatsDateTime;
    }

    internal LastestTransactionResponseDto(DebtPayment debtPayment)
    {
        Id = debtPayment.Id;
        Direction = TransactionDirection.Out;
        Type = TransactionType.DebtPayment;
        Amount = debtPayment.Amount;
        StatsDateTime = debtPayment.StatsDateTime;
    }
}