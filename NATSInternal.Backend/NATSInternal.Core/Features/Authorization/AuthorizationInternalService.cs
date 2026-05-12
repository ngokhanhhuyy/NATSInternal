using JetBrains.Annotations;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Debts;
using NATSInternal.Core.Features.Expenses;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Authorization;

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
            CanUpdate = CanUpdateUser(user),
            CanDelete = CanDeleteUser(user)
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

    public ProductCategoryExistingAuthorizationResponseDto GetProductCategoryExistingAuthorization(
        ProductCategory category)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditProductCategory),
            CanDelete = CallerHasPermission(PermissionNames.DeleteProductCategory)
        };
    }

    public ExpenseExistingAuthorizationResponseDto GetExpenseExistingAuthorization(Expense expense)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditExpense),
            CanDelete = CallerHasPermission(PermissionNames.DeleteExpense)
        };
    }

    public SupplyExistingAuthorizationResponseDto GetSupplyExistingAuthorization(Supply supply)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditSupply),
            CanDelete = CallerHasPermission(PermissionNames.DeleteSupply)
        };
    }

    public OrderExistingAuthorizationResponseDto GetOrderExistingAuthorization(Order order)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditOrder),
            CanDelete = CallerHasPermission(PermissionNames.DeleteOrder)
        };
    }

    public PaymentExistingAuthorizationResponseDto GetPaymentExistingAuthorization(Payment order)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditPayment),
            CanDelete = CallerHasPermission(PermissionNames.DeletePayment)
        };
    }

    public DebtExistingAuthorizationResponseDto GetDebtExistingAuthorization(Debt debt)
    {
        return new()
        {
            CanEdit = CallerHasPermission(PermissionNames.EditDebt),
            CanDelete = CallerHasPermission(PermissionNames.DeleteDebt)
        };
    }

    public bool CanCreateUser()
    {
        return CallerHasPermission(PermissionNames.CreateUser);
    }

    public bool CanChangeUserPassword(User user)
    {
        return user.DeletedDateTime is null && user.Id == _callerDetailProvider.GetId();
    }

    public bool CanResetUserPassword(User user)
    {
        return user.DeletedDateTime is not null &&
            user.Id != _callerDetailProvider.GetId() &&
            CallerHasPermission(PermissionNames.ResetAnotherUserPassword);
    }

    public bool CanUpdateUser(User user)
    {
        return user.Id != _callerDetailProvider.GetId() &&
            user.DeletedDateTime is null &&
            CallerHasPermission(PermissionNames.UpdateAnotherUser);
    }

    public bool CanDeleteUser(User user)
    {
        return user.Id != _callerDetailProvider.GetId() &&
            user.DeletedDateTime is null &&
            CallerHasPermission(PermissionNames.DeleteAnotherUser);
    }

    public bool CanRestoreUser(User user)
    {
        return user.Id != _callerDetailProvider.GetId() &&
            user.DeletedDateTime is not null &&
            CallerHasPermission(PermissionNames.RestoreAnotherUser);
    }

    public bool CanAddUserToRole(User user, Role role)
    {
        return CanUpdateUser(user)
            && CallerPowerLevel() > user.MaxRolePowerLevel
            && CallerPowerLevel() >= role.PowerLevel;
    }

    public bool CanRemoveUserFromRole(User user, Role role)
    {
        return CanUpdateUser(user)
            && CallerPowerLevel() > user.MaxRolePowerLevel
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

    public bool CanCreateProductCategory()
    {
        return CallerHasPermission(PermissionNames.CreateProductCategory);
    }

    public bool CanCreateExpense()
    {
        return CallerHasPermission(PermissionNames.CreateExpense);
    }

    public bool CanCreateSupply()
    {
        return CallerHasPermission(PermissionNames.CreateSupply);
    }

    public bool CanCreateOrder()
    {
        return CallerHasPermission(PermissionNames.CreateOrder);
    }

    public bool CanCreatePayment()
    {
        return CallerHasPermission(PermissionNames.CreatePayment);
    }

    public bool CanCreateDebt()
    {
        return CallerHasPermission(PermissionNames.CreateDebt);
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