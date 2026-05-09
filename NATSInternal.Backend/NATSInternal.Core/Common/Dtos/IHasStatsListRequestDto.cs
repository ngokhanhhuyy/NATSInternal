namespace NATSInternal.Core.Common.Dtos;

public interface IHasStatsListRequestDto : IListRequestDto
{
    #region Properties
    ListMonthYearRequestDto? StatsMonthYear { get; set; }
    #endregion
}