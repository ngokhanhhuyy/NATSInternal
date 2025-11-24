using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NATSInternal.Application.AuditLogs;
using NATSInternal.Application.Security;
using NATSInternal.Application.Services;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Repositories;
using NATSInternal.Infrastructure.Security;
using NATSInternal.Infrastructure.Seeders;
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
            // options.UseNpgsql(connectionString, x => x.MigrationsAssembly("NATSInternal.Infrastructure"));
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                x => x.MigrationsAssembly("NATSInternal.Infrastructure"));
        });

        // DbContext.
        services.AddDbContext<AppDbContext>();

        // DbException converters.
        services.AddScoped<IDbExceptionConverter, PostgreSqlExceptionConverter>();

        // Repositories.
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();

        // Services.
        services.AddScoped<IListFetchingService, ListFetchingService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUserService, UserService>();
        
        // Seeders.
        services.AddTransient<Seeder>();
        services.AddTransient<UserSeeder>();
        services.AddTransient<CustomerSeeder>();
        services.AddTransient<ProductSeeder>();
        services.AddTransient<StockSeeder>();

        // Unit of work.
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        // Security.
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // Time.
        services.AddScoped<IClock, Clock>();
    }

    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
    {
        
        IServiceScopeFactory serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
        AppDbContext context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        ILogger<AppDbContext> logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        logger.LogInformation("Ensuring database created");

        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();
        await context.Database.CloseConnectionAsync();
    }

    public static async Task SeedDataAsync(this IServiceProvider serviceProvider, bool isDevelopment)
    {
        IServiceScopeFactory serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
        Seeder seeder = serviceScope.ServiceProvider.GetRequiredService<Seeder>();
        
        await seeder.SeedAsync(isDevelopment);
    }
    #endregion
}