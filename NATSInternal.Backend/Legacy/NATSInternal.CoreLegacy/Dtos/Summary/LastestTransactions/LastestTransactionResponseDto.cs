namespace NATSInternal.Core.Dtos;

public class LastestTransactionResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public TransactionDirection Direction { get; set; }
    public TransactionType Type { get; set; }
    public long Amount { get; set; }
    public DateTime StatsDateTime { get; set; }
    #endregion

    #region Constructors
    internal LastestTransactionResponseDto(Supply supply)
    {
        Id = supply.Id;
        Direction = TransactionDirection.Out;
        Type = TransactionType.Supply;
        Amount = supply.CachedAmount;
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

    internal LastestTransactionResponseDto(Order order)
    {
        Id = order.Id;
        Direction = TransactionDirection.In;
        Amount = order.CachedAmountAfterVat;
        StatsDateTime = order.StatsDateTime;

        switch (order.Type)
        {
            case OrderType.Consultant:
                Type = TransactionType.Consultant;
                break;
            case OrderType.Retail:
                Type = TransactionType.Retail;
                break;
            case OrderType.Treatment:
                Type = TransactionType.Treatment;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    internal LastestTransactionResponseDto(Debt debt)
    {
        Id = debt.Id;
        Direction = TransactionDirection.In;
        Type = debt.Type == DebtType.Incurrence ? TransactionType.DebtIncurrence : TransactionType.DebtPayment;
        Amount = debt.Amount;
        StatsDateTime = debt.StatsDateTime;
    }
    #endregion
}