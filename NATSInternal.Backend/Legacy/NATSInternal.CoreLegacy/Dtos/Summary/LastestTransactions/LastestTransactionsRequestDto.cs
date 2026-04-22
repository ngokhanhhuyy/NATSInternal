namespace NATSInternal.Core.Dtos;

public class LatestTransactionsRequestDto : IRequestDto
{
    #region Properties
    public int Count { get; set; } = 5;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}