namespace NATSInternal.Application.Authorization;

public interface IAuthorizationService
{
    bool CanCreateUser();
    bool CanCreateProduct();
    bool CanCreateProductCategory();
    bool CanCreateBrand();
}