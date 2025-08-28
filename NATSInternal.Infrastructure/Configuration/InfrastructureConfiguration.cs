using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Application.Security;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Repositories;
using NATSInternal.Infrastructure.Security;

namespace NATSInternal.Infrastructure.Configuration;

public static class InfrastructureConfiguration
{
    #region ExtensionMethods
    public static void AddInfrastructureServices(
        this IServiceCollection services,
        string connectionString,
        string fileStoragePath)
    {
        services.AddDbContextFactory<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, x => x.MigrationsAssembly("NATSInternal.Infrastructure"));
        });

        // DbContext.
        services.AddDbContext<AppDbContext>();

        // Repositories.
        services.AddScoped<ListFetcher>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Security.
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
    #endregion
}