namespace NATSInternal.Core.Dtos;

public class UserBasicResponseDto : IUpsertableBasicResponseDto<UserBasicAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; internal set; }
    public string UserName { get; internal set; }
    public ICollection<RoleMinimalResponseDto> Roles { get; internal set; }
    public UserBasicAuthorizationResponseDto? Authorization { get; internal set; }
    public string? ThumbnailUrl { get; internal set; }
    #endregion

    #region Constructors
    internal UserBasicResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new RoleMinimalResponseDto(r)).ToList();
        ThumbnailUrl = user.ThumbnailUrl;
    }

    internal UserBasicResponseDto(User user, UserBasicAuthorizationResponseDto authorizationResponseDto) : this(user)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion
}