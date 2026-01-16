using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Authentication;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Metadata;
using NATSInternal.Application.UseCases.Photos;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Application.UseCases.Users;
using NATSInternal.Application.Validation;

namespace NATSInternal.Application.Configuration;

public static class ApplicationConfiguration
{
    #region ExtensionMethods
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Fluent validation.
        services.AddValidatorsFromAssemblyContaining<VerifyUserNameAndPasswordValidator>(includeInternalTypes: true);
        services.AddScoped<IValidator<UserAddToRolesRequestDto>, UserAddToRolesValidator>();
        services.AddScoped<IValidator<UserRemoveFromRolesRequestDto>, UserRemoveFromRolesValidator>();
        ValidatorOptions.Global.LanguageManager.Enabled = true;
        ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
        {
            Culture = new("vi")
        };

        // UseCases.
        // Authentication UseCases.
        services.AddTransient<IRequestHandler<ChangePasswordRequestDto>, ChangePasswordHandler>();
        services.AddTransient<IRequestHandler<VerifyUserNameAndPasswordRequestDto>, VerifyUserNameAndPasswordHandler>();

        // User UseCases.
        services.AddTransient<IRequestHandler<UserGetListRequestDto, UserGetListResponseDto>, UserGetListHandler>();
        services.AddTransient<
            IRequestHandler<UserGetDetailByIdRequestDto, UserGetDetailResponseDto>,
            UserGetDetailByIdHandler>();
        services.AddTransient<
            IRequestHandler<UserGetDetailByUserNameRequestDto, UserGetDetailResponseDto>,
            UserGetDetailByUserNameHandler>();
        services.AddTransient<IRequestHandler<UserCreateRequestDto, Guid>, UserCreateHandler>();
        services.AddTransient<IRequestHandler<UserAddToRolesRequestDto>, UserAddToRolesHandler>();
        services.AddTransient<IRequestHandler<UserRemoveFromRolesRequestDto>, UserRemoveFromRolesHandler>();
        services.AddTransient<IRequestHandler<UserDeleteRequestDto>, UserDeleteHandler>();

        // Customer UseCases.
        services.AddTransient<
            IRequestHandler<CustomerGetListRequestDto, CustomerGetListResponseDto>,
            CustomerGetListHandler>();
        services.AddTransient<
            IRequestHandler<CustomerGetDetailRequestDto, CustomerGetDetailResponseDto>,
            CustomerGetDetailHandler>();
        services.AddTransient<IRequestHandler<CustomerCreateRequestDto, Guid>, CustomerCreateHandler>();
        services.AddTransient<IRequestHandler<CustomerUpdateRequestDto>, CustomerUpdateHandler>();
        services.AddTransient<IRequestHandler<CustomerDeleteRequestDto>, CustomerDeleteHandler>();
        services.AddTransient<IRequestHandler<CustomerValidateRequestDto>, CustomerValidateHandler>();

        // Product UseCases.
        services.AddTransient<
            IRequestHandler<ProductGetListRequestDto, ProductGetListResponseDto>,
            ProductGetListHandler>();
        services.AddTransient<
            IRequestHandler<ProductGetDetailRequestDto, ProductGetDetailResponseDto>,
            ProductGetDetailHandler>();
        services.AddTransient<IRequestHandler<ProductCreateRequestDto, Guid>, ProductCreateHandler>();
        services.AddTransient<IRequestHandler<ProductUpdateRequestDto>, ProductUpdateHandler>();
        services.AddTransient<IRequestHandler<ProductDeleteRequestDto>, ProductDeleteHandler>();
        services.AddTransient<
            IRequestHandler<BrandGetListRequestDto, BrandGetListResponseDto>,
            BrandGetListHandler>();
        services.AddTransient<
            IRequestHandler<BrandGetAllRequestDto, IEnumerable<BrandBasicResponseDto>>,
            BrandGetAllHandler>();
        services.AddTransient<
            IRequestHandler<ProductCategoryGetAllRequestDto, IEnumerable<ProductCategoryBasicResponseDto>>,
            ProductCategoryGetAllHandler>();
        services.AddTransient<
            IRequestHandler<ProductCategoryGetListRequestDto, ProductCategoryGetListResponseDto>,
            ProductCategoryGetListHandler>();


        // Photo UseCases.
        services.AddTransient<
            IRequestHandler<PhotoGetSingleRequestDto, PhotoBasicResponseDto>,
            PhotoGetSingleHandler>();
        services.AddTransient<
            IRequestHandler<PhotoGetMultipleByProductIdsRequestDto, ICollection<PhotoBasicResponseDto>>,
            PhotoGetMultipleByProductIdsHandler>();

        // Metadata UseCases.
        services.AddTransient<IRequestHandler<MetadataGetRequestDto, MetadataGetResponseDto>, MetadataGetHandler>();

        // Services.
        services.AddScoped<IAuthorizationService, AuthorizationInternalService>();
        services.AddScoped<IAuthorizationInternalService, AuthorizationInternalService>();
    }
    #endregion
}