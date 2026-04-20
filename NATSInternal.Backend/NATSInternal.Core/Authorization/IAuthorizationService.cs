namespace NATSInternal.Core.Common.Authorization;

public interface IAuthorizationService
{
    bool CanCreateUser();
    bool CanCreateCustomer();
    bool CanCreateProduct();
    bool CanCreateProductCategory();
    bool CanCreateBrand();
    bool CanCreateSupply();
}