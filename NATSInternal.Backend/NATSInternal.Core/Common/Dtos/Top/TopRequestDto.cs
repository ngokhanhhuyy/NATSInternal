using NATSInternal.Core.Common.Enums;

namespace NATSInternal.Core.Common.Dtos;

public class TopRequestDto : ITopAndCountRequestDto
{
    #region Properties
    public int ResultsCount { get; set; }
    public TimeRangeUnitType TimeRangeUnitType { get; set; }
    public int TimeRangeUnitCount { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
