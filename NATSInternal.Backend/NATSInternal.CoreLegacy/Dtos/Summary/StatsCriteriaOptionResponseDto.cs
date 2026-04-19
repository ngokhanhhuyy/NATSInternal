namespace NATSInternal.Core.Dtos;

public class StatsCriteriaOptionResponseDto
{
    public string Name { get; private set; }
    public string DisplayName => DisplayNames.Get(Name);

    public StatsCriteriaOptionResponseDto(string name)
    {
        Name = name;
    }
}