namespace NATSInternal.Application.Authorization;

public interface IAuthorizationService
{
    bool CanCreateUser();
    bool CanCreateCustomer();
    bool CanCreateProduct();
    bool CanCreateProductCategory();
    bool CanCreateBrand();
}