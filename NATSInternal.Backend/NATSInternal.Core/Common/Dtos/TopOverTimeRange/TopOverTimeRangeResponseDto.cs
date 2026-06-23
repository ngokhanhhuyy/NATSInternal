namespace NATSInternal.Core.Common.Dtos;

public class TopOverTimeRangeResponseDto<TBasicResponseDto, TMetric>
    : List<TopItemResponseDto<TBasicResponseDto, TMetric>> where TBasicResponseDto: class
{
    #region Constructors
    public TopOverTimeRangeResponseDto(List<TopItemResponseDto<TBasicResponseDto, TMetric>> items) : base(items) { }
    #endregion
}
