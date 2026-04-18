namespace NATSInternal.Application.UseCases;

public interface ITransactionListRequestDto : IListRequestDto
{
    #region Properties
    DateTime OccurredRangeStartingDateTime { get; }
    DateTime OccurredRangeEndingDateTime { get; }
    #endregion
}
