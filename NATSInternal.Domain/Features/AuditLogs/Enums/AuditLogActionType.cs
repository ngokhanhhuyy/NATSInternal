namespace NATSInternal.Domain.Features.AuditLogs;

public enum AuditLogActionType
{
    #region Elements
    UserCreate,
    UserResetPassword,
    UserAddToRoles,
    UserRemoveFromRoles,
    UserRemove,

    ProductCreate,
    ProductUpdate,
    ProductRemove,

    BrandCreate,
    BrandUpdate,
    BrandRemove,

    ProductCategoryUpdate,
    ProductCategoryRemove
    #endregion
}
