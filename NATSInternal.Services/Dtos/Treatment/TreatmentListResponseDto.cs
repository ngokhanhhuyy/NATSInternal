namespace NATSInternal.Services.Dtos;

public class TreatmentListResponseDto : IFinancialEngageableListResponseDto<
        TreatmentBasicResponseDto,
        TreatmentExistingAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<TreatmentBasicResponseDto> Items { get; set; }
}