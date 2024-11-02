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

    [JsonInclude]
    public static Dictionary<string, string> StaticOrderByFieldOptions { get; }

    static TreatmentListResponseDto()
    {
        List<OrderByFieldOption> options = new List<OrderByFieldOption>
        {
            OrderByFieldOption.Amount,
            OrderByFieldOption.StatsDateTime
        };

        StaticOrderByFieldOptions = options
            .ToDictionary(o => o.ToString(), o => DisplayNames.Get(o.ToString()));
    }
}