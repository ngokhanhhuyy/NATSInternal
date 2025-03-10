namespace NATSInternal.Services.Dtos;

public class DebtPaymentListResponseDto : IHasStatsResponseDto<
        DebtPaymentBasicResponseDto,
        DebtPaymentExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<DebtPaymentBasicResponseDto> Items { get; set; }
}