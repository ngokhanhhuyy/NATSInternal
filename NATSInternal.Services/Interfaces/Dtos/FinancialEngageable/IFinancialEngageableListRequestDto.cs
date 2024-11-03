namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableListRequestDto : ICreatorTrackableListRequestDto
{
    MonthYearRequestDto MonthYear { get; set; }
}