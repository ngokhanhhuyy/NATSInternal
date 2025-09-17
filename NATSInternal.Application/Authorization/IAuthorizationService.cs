using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.Authorization;

internal interface IAuthorizationService
{
    #region Methods
    UserExistingAuthorizationResponseDto GetUserExistingAuthorization(User targetUser);
    ProductExistingAuthorizationResponseDto GetProductExistingAuthorization(Product product);
    
    bool CanCreateUser();
    bool CanChangeUserPassword(User targetUser);
    bool CanResetUserPassword(User targetUser);
    bool CanDeleteUser(User targetUser);
    bool CanAddUserToRole(User user, Role role);
    bool CanRemoveUserFromRole(User user, Role role);
    #endregion
}
