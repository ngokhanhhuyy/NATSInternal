using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetListResponseDto : IListResponseDto<UserGetListUserResponseDto>
{
    #region Constructors
    internal UserGetListResponseDto(
        IEnumerable<UserGetListUserResponseDto> items,
        int pageCount,
        int itemCount)
    {
        Items = items;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion
    
    #region Properties
    public IEnumerable<UserGetListUserResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}

public class UserGetListUserResponseDto : UserBasicResponseDto
{
    #region Constructors
    internal UserGetListUserResponseDto(User user, UserExistingAuthorizationResponseDto authorization) : base(user)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public UserExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}