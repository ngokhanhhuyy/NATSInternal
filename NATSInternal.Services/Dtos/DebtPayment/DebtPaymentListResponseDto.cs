namespace NATSInternal.Services.Dtos;

public class DebtPaymentListResponseDto
    : IFinancialEngageableListResponseDto<
        DebtPaymentBasicResponseDto,
        DebtPaymentAuthorizationResponseDto,
        DebtPaymentListAuthorizationResponseDto>


{
    public List<DebtPaymentBasicResponseDto> Items { get; set; }
    public int PageCount { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public DebtPaymentListAuthorizationResponseDto Authorization { get; set; }
}