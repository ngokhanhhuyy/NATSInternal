namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasStatsListRequestDto : ICreatorTrackableListRequestDto
{
    ListMonthYearRequestDto MonthYear { get; set; }
}