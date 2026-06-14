using NATSInternal.Core.Common.Enums;

namespace NATSInternal.Core.Common.Dtos;

public interface ITopAndCountRequestDto : IRequestDto
{
    #region Properties
    TopTimeRangeUnitType TimeRangeUnitType { get; set; }
    int TimeRangeUnitCount { get; set; }
    #endregion
}
