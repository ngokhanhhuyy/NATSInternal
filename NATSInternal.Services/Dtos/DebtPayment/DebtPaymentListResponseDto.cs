namespace NATSInternal.Services.Dtos;

public class DebtPaymentListResponseDto : IFinancialEngageableListResponseDto<
        DebtPaymentBasicResponseDto,
        DebtPaymentExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<DebtPaymentBasicResponseDto> Items { get; set; }
}