namespace NATSInternal.Core.Dtos;

public class StatsCriteriaOptionListResponseDto
{
    public IEnumerable<StatsCriteriaOptionResponseDto> Options { get; private set; }
    public string Default { get; private set; }

    public StatsCriteriaOptionListResponseDto(Type enumType, Enum defaultOption)
    {
        Options = Enum
            .GetNames(enumType)
            .Select(name => new StatsCriteriaOptionResponseDto(name));
        Default = defaultOption.ToString();
    }
}