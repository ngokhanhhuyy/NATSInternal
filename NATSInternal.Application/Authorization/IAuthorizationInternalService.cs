using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.Authorization;

internal interface IAuthorizationInternalService : IAuthorizationService
{
    #region Methods
    UserExistingAuthorizationResponseDto GetUserExistingAuthorization(User targetUser);
    CustomerExistingAuthorizationResponseDto GetCustomerExistingAuthorization(Customer customer);
    ProductExistingAuthorizationResponseDto GetProductExistingAuthorization(Product product);
    BrandExistingAuthorizationResponseDto GetBrandExistingAuthorization(Brand brand);

    bool CanChangeUserPassword(User targetUser);
    bool CanResetUserPassword(User targetUser);
    bool CanDeleteUser(User targetUser);
    bool CanAddUserToRole(User user, Role role);
    bool CanRemoveUserFromRole(User user, Role role);
    #endregion
}
