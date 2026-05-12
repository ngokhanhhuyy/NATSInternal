using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Debts;
using NATSInternal.Core.Features.Expenses;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Features.Authorization;

internal interface IAuthorizationInternalService : IAuthorizationService
{
    #region Methods
    UserExistingAuthorizationResponseDto GetUserExistingAuthorization(User targetUser);
    CustomerExistingAuthorizationResponseDto GetCustomerExistingAuthorization(Customer customer);
    ProductExistingAuthorizationResponseDto GetProductExistingAuthorization(Product product);
    ProductCategoryExistingAuthorizationResponseDto GetProductCategoryExistingAuthorization(ProductCategory category);
    ExpenseExistingAuthorizationResponseDto GetExpenseExistingAuthorization(Expense expense);
    SupplyExistingAuthorizationResponseDto GetSupplyExistingAuthorization(Supply supply);
    OrderExistingAuthorizationResponseDto GetOrderExistingAuthorization(Order order);
    PaymentExistingAuthorizationResponseDto GetPaymentExistingAuthorization(Payment payment);
    DebtExistingAuthorizationResponseDto GetDebtExistingAuthorization(Debt debt);
    bool CanChangeUserPassword(User targetUser);
    bool CanResetUserPassword(User targetUser);
    bool CanDeleteUser(User targetUser);
    bool CanRestoreUser(User targetUser);
    bool CanAddUserToRole(User user, Role role);
    bool CanRemoveUserFromRole(User user, Role role);
    #endregion
}
