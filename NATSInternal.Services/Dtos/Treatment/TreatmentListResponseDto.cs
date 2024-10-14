namespace NATSInternal.Services.Dtos;

public class TreatmentListResponseDto : IFinancialEngageableListResponseDto<
        TreatmentBasicResponseDto,
        TreatmentAuthorizationResponseDto,
        TreatmentListAuthorizationResponseDto>
{
    public int PageCount { get; set; }
    public List<TreatmentBasicResponseDto> Items { get; set; }
    public List<MonthYearResponseDto> MonthYearOptions { get; set; }
    public TreatmentListAuthorizationResponseDto Authorization { get; set; }
}