using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Authentication;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Products.ProductGetList;
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
            Culture = new System.Globalization.CultureInfo("vi")
        };

        // ValidatorOptions.Global.PropertyNameResolver = (_, b, _) =>
        // {
        //     string? name = 
        //     return b.Name.FirstOrDefault()?.ToString().ToLower() + b.Name[1..] ?? 
        // }

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

        // Product UseCases.
        services.AddTransient<
            IRequestHandler<ProductGetListRequestDto, ProductGetListResponseDto>,
            ProductGetListHandler>();
        services.AddTransient<
            IRequestHandler<ProductGetDetailRequestDto, ProductGetDetailResponseDto>,
            ProductGetDetailHandler>();

        // Services.
        services.AddScoped<IAuthorizationService, AuthorizationInternalService>();
        services.AddScoped<IAuthorizationInternalService, AuthorizationInternalService>();
    }
    #endregion
}
