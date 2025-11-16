using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Shared;

public class UserBasicResponseDto
{
    #region Constructors
    internal UserBasicResponseDto(User? user)
    {
        if (user is not null && !user.DeletedDateTime.HasValue)
        {
            Id = user.Id;
            UserName = user.UserName;
            Roles = user.Roles.Select(r => new RoleBasicResponseDto(r)).ToList();
        }
        else
        {
            Id = Guid.Empty;
            UserName = string.Empty;
            Roles = new List<RoleBasicResponseDto>();
            IsDeleted = true;
        }
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
    public bool IsDeleted { get; }
    #endregion
}