using NATSInternal.Core.Common.Enums;

namespace NATSInternal.Core.Common.Dtos;

public class CountRequestDto : ITopAndCountRequestDto
{
    #region Properties
    public TopTimeRangeUnitType TimeRangeUnitType { get; set; }
    public int TimeRangeUnitCount { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
