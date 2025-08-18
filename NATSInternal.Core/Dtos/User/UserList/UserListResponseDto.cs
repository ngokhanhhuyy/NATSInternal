namespace NATSInternal.Core.Dtos;

public class UserListResponseDto : IPageableListResponseDto<UserBasicResponseDto>
{
    #region Properties
    public int PageCount { get; set; } = 0;
    public List<UserBasicResponseDto> Items { get; set; } = new();
    #endregion
}