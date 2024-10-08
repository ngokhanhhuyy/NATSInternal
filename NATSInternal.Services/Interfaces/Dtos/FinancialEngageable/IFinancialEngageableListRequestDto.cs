namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableListRequestDto
    : ICreatorTrackableListRequestDto
{
    int Month { get; set; }
    int Year { get; set; }
    bool IgnoreMonthYear { get; set; }
}