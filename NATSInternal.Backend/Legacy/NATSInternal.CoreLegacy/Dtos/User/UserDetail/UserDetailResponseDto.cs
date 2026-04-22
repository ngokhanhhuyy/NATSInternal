namespace NATSInternal.Core.Dtos;

public class UserDetailResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public List<RoleDetailResponseDto> Roles { get; set; }
    public UserDetailAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal UserDetailResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new RoleDetailResponseDto(r)).ToList();
    }

    internal UserDetailResponseDto(User user, UserDetailAuthorizationResponseDto authorization) : this(user)
    {
        Authorization = authorization;
    }
    #endregion
}