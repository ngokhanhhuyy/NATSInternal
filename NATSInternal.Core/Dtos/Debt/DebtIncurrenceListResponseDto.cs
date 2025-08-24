namespace NATSInternal.Core.Dtos;

public class DebtIncurrenceListResponseDto : IHasStatsResponseDto<
        DebtBasicResponseDto,
        DebtExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<DebtBasicResponseDto> Items { get; set; }
}