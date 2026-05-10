using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Test.Mock;

namespace NATSInternal.Test.Extensions;

public static class IServiceScopeExtensions
{
    #region ExtensionMethods
    extension(IServiceScope scope)
    {
        public async Task InitializeDependenciesAsync(string roleName)
        {
            CallerDetailContext callerDetailContext = scope.ServiceProvider.GetRequiredService<CallerDetailContext>();
            CallerDetailProvider callerDetailProvider;
            callerDetailProvider = scope.ServiceProvider.GetRequiredService<CallerDetailProvider>();
            await callerDetailProvider.InitializeByRoleNameAsync(callerDetailContext, roleName);

            Clock clock = scope.ServiceProvider.GetRequiredService<Clock>();
            clock.SetCurrentDateTime(new(2026, 5, 10, 12, 0, 0));
        }
    }
    #endregion
}