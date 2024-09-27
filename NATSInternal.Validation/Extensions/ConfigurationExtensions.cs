using System.Globalization;

namespace NATSInternal.Validation.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureFluentValidation(
            this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<SignInValidator>();
        ValidatorOptions.Global.LanguageManager.Enabled = true;
        ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
        {
            Culture = new CultureInfo("vi")
        };

        ValidatorOptions.Global.PropertyNameResolver = (_, b, _) => b.Name
            .First()
            .ToString()
            .ToLower() + b.Name[1..];

        return services;
    }
}