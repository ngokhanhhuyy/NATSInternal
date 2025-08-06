namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasStatsListRequestDto : ICreatorTrackableListRequestDto
{
    ListMonthYearRequestDto MonthYear { get; set; }
}