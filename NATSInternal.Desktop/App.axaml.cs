using System.Reflection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Logging;
using NATSInternal.Application.Configuration;
using NATSInternal.Application.Security;
using NATSInternal.Desktop.Configurations;
using NATSInternal.Desktop.Sessions;
using NATSInternal.Desktop.Views;
using NATSInternal.Desktop.ViewModels;
using NATSInternal.Infrastructure.Configuration;

namespace NATSInternal.Desktop;

public partial class App : Avalonia.Application
{
    #region Fields
    private static IServiceProvider? _rootServiceProvider;
    #endregion
    
    #region Constructors
    public App()
    {
        IServiceCollection services = new ServiceCollection();
        
        // Add application layer.
        services.AddApplicationServices();
        
        // Add infrastructure layer.
        services.AddInfrastructureServices(
            "Host=localhost;Port=3306;Database=natsinternal;Username=root;Password=Huyy47b1",
            string.Empty);
        services.AddViewModels();
        
        // Add loggers.
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.SetMinimumLevel(LogLevel.Information);
        });
        
        // Add MediatR.
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        // Add session.
        services.AddSingleton<ICallerDetailProvider, CurrentUserSession>();

        _rootServiceProvider = services.BuildServiceProvider();
        _rootServiceProvider.EnsureDatabaseCreatedAsync().GetAwaiter().GetResult();
        _rootServiceProvider.SeedDataAsync(true).GetAwaiter().GetResult();
    }
    #endregion
    
    #region Properties
    public static IServiceProvider RootServiceProvider => _rootServiceProvider
        ?? throw new InvalidOperationException("Root service provider hasn't been initialized yet.");
    #endregion
    
    #region Methods
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    #endregion
    
    #region PrivateMethods
    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
    #endregion
}