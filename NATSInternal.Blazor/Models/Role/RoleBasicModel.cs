namespace NATSInternal.Blazor.Models;

public class RoleBasicModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public int PowerLevel { get; set; }

    public RoleBasicModel(RoleBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        DisplayName = responseDto.DisplayName;
        PowerLevel = responseDto.PowerLevel;
    }

    public RoleRequestDto ToRequestDto()
    {
        return new RoleRequestDto
        {
            Name = Name
        };
    }
}