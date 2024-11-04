namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableListRequestDto : ICreatorTrackableListRequestDto
{
    ListMonthYearRequestDto MonthYear { get; set; }
}