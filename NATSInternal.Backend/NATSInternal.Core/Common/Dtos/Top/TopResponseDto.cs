namespace NATSInternal.Core.Common.Dtos;

public class TopResponseDto<TBasicResponseDto, TMetric> : List<TopItemResponseDto<TBasicResponseDto, TMetric>>
    where TBasicResponseDto: class
{
    #region Constructors
    public TopResponseDto(List<TopItemResponseDto<TBasicResponseDto, TMetric>> items) : base(items) { }
    #endregion
}
