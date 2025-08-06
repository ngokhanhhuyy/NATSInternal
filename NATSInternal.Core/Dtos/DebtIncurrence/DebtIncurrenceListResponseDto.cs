namespace NATSInternal.Core.Dtos;

public class DebtIncurrenceListResponseDto : IHasStatsResponseDto<
        DebtIncurrenceBasicResponseDto,
        DebtIncurrenceExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<DebtIncurrenceBasicResponseDto> Items { get; set; }
}