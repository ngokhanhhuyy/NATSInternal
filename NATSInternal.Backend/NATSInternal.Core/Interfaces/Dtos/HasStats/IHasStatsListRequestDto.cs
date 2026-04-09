namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasStatsListRequestDto : ICreatorTrackableListRequestDto
{
    #region Properties
    ListMonthYearRequestDto? MonthYear { get; set; }
    #endregion
}