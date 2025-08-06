namespace NATSInternal.Core.Dtos;

public class TreatmentListResponseDto : IHasStatsResponseDto<
        TreatmentBasicResponseDto,
        TreatmentExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<TreatmentBasicResponseDto> Items { get; set; }
}