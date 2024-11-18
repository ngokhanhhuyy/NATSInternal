namespace NATSInternal.Blazor.Models;

public class RoleMinimalModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }

    public RoleMinimalModel(RoleMinimalResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        DisplayName = responseDto.DisplayName;
    }
}