namespace NATSInternal.Core.Common.Dtos;

public class CountOverTimeRangeResponseDto<TMetric> : IRequestDto
{
    #region Constructors
    public CountOverTimeRangeResponseDto(
        TMetric currentTimeRangeCount,
        TMetric previousTimeRangeCount,
        int percentageComparedToPreviousTimeRange)
    {
        CurrentTimeRangeCount = currentTimeRangeCount;
        PreviousTimeRangeCount = previousTimeRangeCount;
        PercentageDiffComparedToPreviousTimeRange = percentageComparedToPreviousTimeRange;
    }
    #endregion

    #region Properties
    public TMetric CurrentTimeRangeCount { get; set; }
    public TMetric PreviousTimeRangeCount { get; set; }
    public int PercentageDiffComparedToPreviousTimeRange { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
