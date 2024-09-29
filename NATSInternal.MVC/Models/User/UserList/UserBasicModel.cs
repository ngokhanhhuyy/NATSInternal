using NATSInternal.Helpers;

namespace NATSInternal.Models;

public class UserBasicModel
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
    public RoleBasicModel Role { get; set; }

    public UserBasicModel(UserBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        UserName = responseDto.UserName;
        FirstName = responseDto.FirstName;
        MiddleName = responseDto.MiddleName;
        LastName = responseDto.LastName;
        FullName = responseDto.FullName;
        Gender = responseDto.Gender;
        Birthday = responseDto.Birthday;
        JoiningDate = responseDto.JoiningDate;

        AvatarUrl = responseDto.AvatarUrl;
        AvatarUrl ??= $"https://ui-avatars.com/api/?name={FullName.Replace(" ", "+")}" +
            "&background=random&size=256";;
        Role = RoleBasicModel.FromResponseDto(responseDto.Role);
    }
}