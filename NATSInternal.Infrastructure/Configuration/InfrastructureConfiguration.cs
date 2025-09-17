using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Application.Security;
using NATSInternal.Application.Services;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Repositories;
using NATSInternal.Infrastructure.Security;
using NATSInternal.Infrastructure.Services;
using NATSInternal.Infrastructure.Time;

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

        // DbException converters.
        services.AddScoped<IDbExceptionConverter, PostgreSqlExceptionConverter>();

        // Repositories.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        // Services.
        services.AddScoped<IListFetchingService, ListFetchingService>();
        services.AddScoped<

        // Unit of work.
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Security.
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        
        // Time.
        services.AddScoped<IClock, Clock>();
    }
    #endregion
}