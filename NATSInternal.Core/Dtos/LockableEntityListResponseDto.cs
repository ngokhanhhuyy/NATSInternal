namespace NATSInternal.Core.Dtos;

public class LockableEntityListResponseDto<TBasicResponseDto> : ListResponseDto<TBasicResponseDto>
{
    public List<ListMonthYearResponseDto> MonthYearOptions { get; set; }
}
