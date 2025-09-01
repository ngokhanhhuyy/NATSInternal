using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Application.UseCases.Authentication;
using NATSInternal.Application.Validation;

namespace NATSInternal.Application.Configuration;

public static class ApplicationConfiguration
{
    #region ExtensionMethods
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Fluent validation.
        services.AddValidatorsFromAssemblyContaining<VerifyUserNameAndPasswordValidator>(includeInternalTypes: true);
        ValidatorOptions.Global.LanguageManager.Enabled = true;
        ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
        {
            Culture = new System.Globalization.CultureInfo("vi")
        };

        ValidatorOptions.Global.PropertyNameResolver = (_, b, _) => b.Name.First().ToString().ToLower() + b.Name[1..];

        // MediatR.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(VerifyUserNameAndPasswordHandler).Assembly);
        });
    }
    #endregion
}
