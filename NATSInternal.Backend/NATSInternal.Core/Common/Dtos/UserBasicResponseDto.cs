using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Common.Dtos;

public class UserBasicResponseDto
{
    #region Constructors
    internal UserBasicResponseDto(User user)
    {
        Id = user.Id;
        UserName = user.UserName;
        
    }
    #endregion
    
    #region Properties
    public int Id { get; }
    public string UserName { get; }
    #endregion
}