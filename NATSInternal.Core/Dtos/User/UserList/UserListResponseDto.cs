namespace NATSInternal.Core.Dtos;

public class UserListResponseDto : IPageableListResponseDto<UserBasicResponseDto>
{
    #region Constructors
    internal UserListResponseDto(ICollection<User> users, int pageCount)
    {
        Items.AddRange(users.Select(u => new UserBasicResponseDto(u)));
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public List<UserBasicResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; } = 0;
    #endregion
}