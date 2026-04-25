using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Users;

public class UserUpdateRequestDto : IRequestDto
{
    #region Properties
    public required List<int> RoleIds { get; set; } 
    #endregion
    
    #region Methods
    public void TransformValues() { }
    #endregion
}