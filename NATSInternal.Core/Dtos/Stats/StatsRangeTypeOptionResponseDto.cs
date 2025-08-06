namespace NATSInternal.Core.Dtos;

public class StatsRangeTypeOptionResponseDto
{
    public string Name { get; set; }
    public string DisplayName => DisplayNames.Get(Name);

    public StatsRangeTypeOptionResponseDto(string name)
    {
        Name = name;
    }
}