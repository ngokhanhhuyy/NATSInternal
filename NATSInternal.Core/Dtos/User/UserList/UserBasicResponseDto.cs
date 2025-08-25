namespace NATSInternal.Core.Dtos;

public class UserBasicResponseDto : IUpsertableBasicResponseDto<UserBasicAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; internal set; }
    public string UserName { get; internal set; }
    public ICollection<RoleMinimalResponseDto> Roles { get; internal set; }
    public AnnouncementExistingAuthorizationResponseDto? Authorization { get; internal set; }
    #endregion

    #region Constructors
    internal UserBasicResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new RoleMinimalResponseDto(r)).ToList();
    }

    internal UserBasicResponseDto(User user, UserBasicAuthorizationResponseDto authorizationResponseDto) : this(user)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion
}