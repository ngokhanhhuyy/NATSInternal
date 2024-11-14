namespace NATSInternal.Services.Dtos;

public class UserBasicResponseDto
        : IUpsertableBasicResponseDto<UserBasicAuthorizationResponseDto>
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public DateOnly? JoiningDate { get; set; }
    public string AvatarUrl { get; set; }
    public RoleMinimalResponseDto Role { get; set; }
    public UserBasicAuthorizationResponseDto Authorization { get; set; }

    [JsonIgnore]
    public string ThumbnailUrl => AvatarUrl;

    internal UserBasicResponseDto(User user)
    {
        MapFromEntity(user);
    }

    internal UserBasicResponseDto(
            User user,
            UserBasicAuthorizationResponseDto authorizationResponseDto)
    {
        MapFromEntity(user);
        Authorization = authorizationResponseDto;
    }

    private void MapFromEntity(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        FirstName = user.FirstName;
        MiddleName = user.MiddleName;
        LastName = user.LastName;
        FullName = user.FullName;
        Gender = user.Gender;
        Birthday = user.Birthday;
        JoiningDate = user.JoiningDate;
        AvatarUrl = user.AvatarUrl;
        Role = new RoleMinimalResponseDto(user.Role);
    }
}