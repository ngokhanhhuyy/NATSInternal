using System.Numerics;

namespace NATSInternal.Core.Common.Dtos;

public class TopResponseDto<TBasicResponseDto, TMetric> where TBasicResponseDto : class
{
    #region Constructors
    internal TopResponseDto(TBasicResponseDto item, TMetric metric)
    {
        Item = item;
        Metric = metric;
    }
    #endregion

    #region Properties
    public TBasicResponseDto Item { get; }
    public TMetric Metric { get; }
    #endregion
}
