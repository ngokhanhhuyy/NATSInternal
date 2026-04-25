namespace NATSInternal.Core.Features.Authorization;

public static class PermissionNames
{
    #region Constants
    // Permissions to interact with users.
    public const string CreateUser = nameof(CreateUser);
    public const string UpdateAnotherUser = nameof(UpdateAnotherUser);
    public const string ResetAnotherUserPassword = nameof(ResetAnotherUserPassword);
    public const string DeleteAnotherUser = nameof(DeleteAnotherUser);

    // Permissions to interact with customers.
    public const string GetCustomerDetail = nameof(GetCustomerDetail);
    public const string CreateCustomer = nameof(CreateCustomer);
    public const string EditCustomer = nameof(EditCustomer);
    public const string DeleteCustomer = nameof(DeleteCustomer);

    // Permissions to interact with products
    public const string CreateProduct = nameof(CreateProduct);
    public const string EditProduct = nameof(EditProduct);
    public const string DeleteProduct = nameof(DeleteProduct);

    // Permissions to interact with product categories
    public const string CreateProductCategory = nameof(CreateProductCategory);
    public const string EditProductCategory = nameof(EditProductCategory);
    public const string DeleteProductCategory = nameof(DeleteProductCategory);

    // Permissions to interact with supplies.
    public const string CreateSupply = nameof(CreateSupply);
    public const string EditSupply = nameof(EditSupply);
    public const string EditLockedSupply = nameof(EditLockedSupply);
    public const string DeleteSupply = nameof(DeleteSupply);
    public const string SetSupplyStatsDateTime = nameof(SetSupplyStatsDateTime);
    public const string AccessSupplyUpdateHistories = nameof(AccessSupplyUpdateHistories);

    // Permissions to interact with expenses.
    public const string CreateExpense = nameof(CreateExpense);
    public const string EditExpense = nameof(EditExpense);
    public const string EditLockedExpense = nameof(EditLockedExpense);
    public const string DeleteExpense = nameof(DeleteExpense);
    public const string SetExpenseStatsDateTime = nameof(SetExpenseStatsDateTime);
    public const string AccessExpenseUpdateHistories = nameof(AccessExpenseUpdateHistories);

    // Permissions to interact with orders.
    public const string CreateOrder = nameof(CreateOrder);
    public const string EditOrder = nameof(EditOrder);
    public const string EditLockedOrder = nameof(EditLockedOrder);
    public const string SetOrderStatsDateTime = nameof(SetOrderStatsDateTime);
    public const string DeleteOrder = nameof(DeleteOrder);
    public const string AccessOrderUpdateHistories = nameof(AccessOrderUpdateHistories);

    // Permissions to interact with debts
    public const string CreateDebt = nameof(CreateDebt);
    public const string EditDebt = nameof(EditDebt);
    public const string EditLockedDebt = nameof(EditLockedDebt);
    public const string SetDebtStatsDateTime = nameof(SetDebtStatsDateTime);
    public const string DeleteDebt = nameof(DeleteDebt);
    public const string AccessDebtUpdateHistories = nameof(AccessDebtUpdateHistories);

    // Permissions to interact with announcements.
    public const string CreateAnnouncement = nameof(CreateAnnouncement);
    public const string EditAnnouncement = nameof(EditAnnouncement);
    public const string DeleteAnnouncement = nameof(DeleteAnnouncement);

    // Permission to interact with reports.
    public const string GetFinancialReport = nameof(GetFinancialReport);
    public const string GetApplicationOperationReport = nameof(GetApplicationOperationReport);
    #endregion
}
