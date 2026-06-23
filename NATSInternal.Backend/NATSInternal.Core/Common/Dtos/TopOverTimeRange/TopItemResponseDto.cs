namespace NATSInternal.Core.Common.Dtos;

public class TopItemResponseDto<TBasicResponseDto, TMetric> where TBasicResponseDto : class
{
    #region Constructors
    internal TopItemResponseDto(TBasicResponseDto item, TMetric metric)
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
