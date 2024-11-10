namespace NATSInternal.Services.Dtos;

public class ExpenseListResponseDto : IHasStatsResponseDto<
        ExpenseBasicResponseDto,
        ExpenseExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<ExpenseBasicResponseDto> Items { get; set; }
}