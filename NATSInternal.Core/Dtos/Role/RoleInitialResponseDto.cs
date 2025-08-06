namespace NATSInternal.Core.Dtos;

public class RoleInitialResponseDto : IHasOptionsInitialResponseDto<RoleMinimalResponseDto>
{
    public string DisplayName => DisplayNames.Role;
    public List<RoleMinimalResponseDto> AllAsOptions { get; init; }
}
