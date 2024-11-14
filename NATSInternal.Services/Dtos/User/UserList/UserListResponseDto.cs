namespace NATSInternal.Services.Dtos;

public class UserListResponseDto : IListResponseDto<UserBasicResponseDto>
{
    public int PageCount { get; set; }
    public List<UserBasicResponseDto> Items { get; set; }
}