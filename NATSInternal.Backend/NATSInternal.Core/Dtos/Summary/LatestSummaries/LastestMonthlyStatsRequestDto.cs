namespace NATSInternal.Core.Dtos;

public class LatestMonthlyStatsRequestDto : IRequestDto
{
    #region Properties
    public int MonthCount { get; set; }
    public bool IncludeThisMonth { get; set; } = true;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
