using JetBrains.Annotations;
using NATSInternal.Application.Security;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.Authorization;

[UsedImplicitly]
internal class AuthorizationInternalService : IAuthorizationInternalService
{
    #region Fields
    private readonly ICallerDetailProvider _callerDetailProvider;
    #endregion

    #region Constructors
    public AuthorizationInternalService(ICallerDetailProvider callerDetailProvider)
    {
        _callerDetailProvider = callerDetailProvider;
    }
    #endregion

    #region Methods
    public UserExistingAuthorizationResponseDto GetUserExistingAuthorization(User user)
    {
        return new()
        {
            CanChangePassword = CanChangeUserPassword(user),
            CanResetPassword = CanResetUserPassword(user),
            CanDelete = CanDeleteUser(user),
            CanAddToOrRemoveFromRoles = CanAddUserToOrRemoveUserFromRoles(user)
        };
    }

    public CustomerExistingAuthorizationResponseDto GetCustomerExistingAuthorization(Customer customer)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditCustomer),
            CanDelete = CallerHasPermission(PermissionNames.DeleteCustomer)
        };
    }

    public ProductExistingAuthorizationResponseDto GetProductExistingAuthorization(Product product)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditProduct),
            CanDelete = CallerHasPermission(PermissionNames.DeleteProduct)
        };
    }

    public bool CanCreateUser()
    {
        return CallerHasPermission(PermissionNames.CreateUser);
    }

    public bool CanChangeUserPassword(User user)
    {
        return user.DeletedDateTime is not null && user.Id == _callerDetailProvider.GetId();
    }

    public bool CanResetUserPassword(User user)
    {
        return user.DeletedDateTime is not null &&
            user.Id != _callerDetailProvider.GetId() &&
            CallerHasPermission(PermissionNames.ResetOtherUserPassword);
    }

    public bool CanDeleteUser(User user)
    {
        return user.Id != _callerDetailProvider.GetId() &&
            user.DeletedDateTime is not null &&
            CallerHasPermission(PermissionNames.DeleteUser);
    }

    public bool CanAddUserToOrRemoveUserFromRoles(User user)
    {
        return user.Id != _callerDetailProvider.GetId() &&
            user.DeletedDateTime is not null &&
            CallerHasPermission(PermissionNames.AddUserToOrRemoveUserFromRoles);
    }

    public bool CanAddUserToRole(User user, Role role)
    {
        return CanAddUserToOrRemoveUserFromRoles(user)
            && CallerPowerLevel() > user.PowerLevel
            && CallerPowerLevel() >= role.PowerLevel;
    }

    public bool CanRemoveUserFromRole(User user, Role role)
    {
        return CanAddUserToOrRemoveUserFromRoles(user)
            && CallerPowerLevel() > user.PowerLevel
            && CallerPowerLevel() > role.PowerLevel;
    }

    public bool CanCreateCustomer()
    {
        return CallerHasPermission(PermissionNames.CreateCustomer);
    }

    public bool CanCreateProduct()
    {
        return CallerHasPermission(PermissionNames.CreateProduct);
    }

    public bool CanCreateBrand()
    {
        return CallerHasPermission(PermissionNames.CreateBrand);
    }

    public bool CanCreateProductCategory()
    {
        return CallerHasPermission(PermissionNames.CreateProductCategory);
    }
    #endregion

    #region PrivateMethods
    private bool CallerHasPermission(string permissionName)
    {
        return _callerDetailProvider.GetPermissionNames().Contains(permissionName);
    }

    private int CallerPowerLevel()
    {
        return _callerDetailProvider.GetPowerLevel();
    }
    #endregion
}