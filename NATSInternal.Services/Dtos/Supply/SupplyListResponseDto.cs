namespace NATSInternal.Services.Dtos;

public class SupplyListResponseDto : IHasStatsResponseDto<
        SupplyBasicResponseDto,
        SupplyExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<SupplyBasicResponseDto> Items { get; set; }
}