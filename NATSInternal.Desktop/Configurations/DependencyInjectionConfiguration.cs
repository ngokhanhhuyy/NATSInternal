using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Desktop.ViewModels;
using NATSInternal.Desktop.Views;

namespace NATSInternal.Desktop.Configurations;

public static class DependencyInjectionConfiguration
{
    #region ExtensionMethods
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        // Window and pages.
        services.AddScoped<MainWindow>();
        services.AddTransient<SignInPage>();
        
        // View models.
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SignInViewModel>();
        
        return services;
    }
    #endregion
}