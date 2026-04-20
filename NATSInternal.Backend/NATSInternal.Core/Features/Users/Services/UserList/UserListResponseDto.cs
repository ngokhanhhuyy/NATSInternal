using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Users;

public class UserListResponseDto : IListResponseDto<UserBasicResponseDto>
{
    #region Constructors
    internal UserListResponseDto(List<UserBasicResponseDto> items, int pageCount, int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion
    
    #region Properties
    public List<UserBasicResponseDto> Items { get; set; }
    public int PageCount { get; set; }
    public int ItemCount { get; set; }
    #endregion
}