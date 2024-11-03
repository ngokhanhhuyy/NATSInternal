namespace NATSInternal.Services.Dtos;

public class ExpenseListResponseDto : IFinancialEngageableListResponseDto<
        ExpenseBasicResponseDto,
        ExpenseExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<ExpenseBasicResponseDto> Items { get; set; }
}