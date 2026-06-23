using NATSInternal.Core.Common.Enums;

namespace NATSInternal.Core.Common.Dtos;

public interface ITopAndCountOverTimeRangeRequestDto : IRequestDto
{
    #region Properties
    TimeRangeUnitType TimeRangeUnitType { get; set; }
    int TimeRangeUnitCount { get; set; }
    #endregion
}
