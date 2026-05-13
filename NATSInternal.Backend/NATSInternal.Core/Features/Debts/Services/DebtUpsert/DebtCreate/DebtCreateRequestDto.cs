namespace NATSInternal.Core.Features.Debts;

public class DebtCreateRequestDto : AbstractDebtUpsertRequestDto
{
    #region Properties
    public int CustomerId { get; set; }
    #endregion
}
