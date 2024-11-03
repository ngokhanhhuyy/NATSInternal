namespace NATSInternal.Services.Dtos;

public class UserListResponseDto
{
    public int PageCount { get; set; }
    public List<UserBasicResponseDto> Items { get; set; }
}