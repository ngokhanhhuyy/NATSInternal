namespace NATSInternal.Core.Common.Dtos;

public interface IHasStatsListRequestDto : IListRequestDto
{
    #region Properties
    int? StatsYear { get; set; }
    int? StatsMonth { get; set; }
    #endregion
}
