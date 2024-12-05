namespace NATSInternal.Services.Dtos;

public class StatsRangeTypeOptionListResponseDto
{
    private static IEnumerable<StatsRangeTypeOptionResponseDto> _options = Enum
        .GetNames(typeof(StatsRangeType))
        .Select(name => new StatsRangeTypeOptionResponseDto(name));

    public IEnumerable<StatsRangeTypeOptionResponseDto> Options => _options;
    public string Default { get; private set; }

    public StatsRangeTypeOptionListResponseDto(StatsRangeType defaultOption)
    {
        Default = defaultOption.ToString();
    }
}