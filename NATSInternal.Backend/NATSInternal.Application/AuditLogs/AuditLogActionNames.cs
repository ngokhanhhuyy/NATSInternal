namespace NATSInternal.Application.AuditLogs;

public static class AuditLogActionNames
{
    #region Constants
    public const string UserCreate = nameof(UserCreate);
    public const string UserResetPassword = nameof(UserResetPassword);
    public const string UserAddToRoles = nameof(UserAddToRoles);
    public const string UserRemoveFromRoles = nameof(UserRemoveFromRoles);
    public const string UserRemove = nameof(UserRemove);

    public const string ProductCreate = nameof(ProductCreate);
    public const string ProductUpdate = nameof(ProductUpdate);
    public const string ProductRemove = nameof(ProductRemove);

    public const string BrandCreate = nameof(BrandCreate);
    public const string BrandUpdate = nameof(BrandUpdate);
    public const string BrandRemove = nameof(BrandRemove);

    public const string ProductCategoryUpdate = nameof(ProductCategoryUpdate);
    public const string ProductCategoryRemove = nameof(ProductCategoryRemove);
    #endregion
}
