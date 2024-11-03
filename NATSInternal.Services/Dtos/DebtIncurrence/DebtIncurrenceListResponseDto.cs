namespace NATSInternal.Services.Dtos;

public class DebtIncurrenceListResponseDto : IFinancialEngageableListResponseDto<
        DebtIncurrenceBasicResponseDto,
        DebtIncurrenceExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<DebtIncurrenceBasicResponseDto> Items { get; set; }
}