using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Shared;

public class UserBasicResponseDto
{
    #region Constructors
    internal UserBasicResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        Roles = user.Roles.Select(r => new RoleBasicResponseDto(r)).ToList();
    }
    
    internal UserBasicResponseDto(
        User user,
        UserExistingAuthorizationResponseDto authorizationResponseDto) : this(user)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public ICollection<RoleBasicResponseDto> Roles { get; }
    public UserExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}