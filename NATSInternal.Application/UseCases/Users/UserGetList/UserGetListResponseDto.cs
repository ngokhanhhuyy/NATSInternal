using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

public class UserGetListResponseDto : IPageableListResponseDto<UserGetListUserResponseDto>
{
    #region Constructors
    internal UserGetListResponseDto(ICollection<UserGetListUserResponseDto> items, int pageCount)
    {
        Items = items;
        PageCount = pageCount;
    }
    #endregion
    
    #region Properties
    public ICollection<UserGetListUserResponseDto> Items { get; }
    public int PageCount { get; }
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