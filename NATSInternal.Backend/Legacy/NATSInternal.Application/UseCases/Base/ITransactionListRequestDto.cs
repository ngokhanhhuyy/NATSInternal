namespace NATSInternal.Application.UseCases;

public interface ITransactionListRequestDto : IListRequestDto
{
    #region Properties
    DateTime? TransactionRangeStartingDateTime { get; }
    DateTime? TransactionRangeEndingDateTime { get; }
    #endregion
}
