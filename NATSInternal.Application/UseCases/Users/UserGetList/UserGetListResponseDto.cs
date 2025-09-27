using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetListResponseDto : IPageableListResponseDto<UserBasicResponseDto>
{
    #region Constructors
    internal UserGetListResponseDto(
        ICollection<UserBasicResponseDto> items,
        int pageCount)
    {
        Items = items;
        PageCount = pageCount;
    }
    #endregion
    
    #region Properties
    public ICollection<UserBasicResponseDto> Items { get; }
    public int PageCount { get; }
    #endregion
}