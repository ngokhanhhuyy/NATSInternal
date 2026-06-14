using NATSInternal.Core.Common.Enums;

namespace NATSInternal.Core.Common.Dtos;

public class TopRequestDto : IRequestDto
{
    #region Properties
    public int ResultsCount { get; set; }
    public TopTimeRangeUnitType TimeRangeUnitType { get; set; }
    public int TimeRangeUnitCount { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
