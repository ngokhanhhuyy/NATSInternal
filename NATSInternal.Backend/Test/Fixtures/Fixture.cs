using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Configurations;
using NATSInternal.Test.Mock;

namespace NATSInternal.Test.Fixtures;

public class Fixture : IDisposable
{
    #region Constructors
    public Fixture()
    {
        ServiceCollection services = new();
        
        services.AddCoreServices(
            "Host=localhost;Port=5432;Database=natsinternal;" +
            "Username=postgres;Password=Huyy47b1;Include Error Detail=true",
            string.Empty
        );

        services.AddScoped<CallerDetailProvider>();
        services.AddScoped<ICallerDetailProvider>(sp => sp.GetRequiredService<CallerDetailProvider>());
        services.AddScoped<CallerDetailContext>();

        services.AddScoped<Clock>();
        services.AddScoped<IClock>(sp => sp.GetRequiredService<Clock>());

        RootProvider = services.BuildServiceProvider();
    }
    #endregion

    #region Properties
    public IServiceProvider RootProvider { get; private set; } = default!;
    #endregion

    #region Methods
    public void Dispose()
    {
        if (RootProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
        GC.SuppressFinalize(this);
    }
    #endregion
}