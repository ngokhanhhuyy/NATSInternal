using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;
using NATSInternal.Core.Persistence.Seeders;
using NATSInternal.Core.Features.Authentication;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Configurations;

public static class CoreConfiguration
{
    extension(IServiceCollection services)
    {
        public void AddCoreServices(string connectionString, string fileStoragePath)
        {
            // DbContextFactory.
            services.AddDbContextFactory<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            
            // DbContext.
            services.AddDbContext<AppDbContext>();

            // DbException handlers.
            services.AddScoped<IDbExceptionHandler, PostgreSqlDbExceptionHandler>();
            
            // Services.
            services.AddScoped<IListFetchingService, ListFetchingService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthorizationInternalService>();
            services.AddScoped<IAuthorizationInternalService>(sp =>
                sp.GetRequiredService<AuthorizationInternalService>());
            services.AddScoped<IAuthorizationService>(sp => sp.GetRequiredService<AuthorizationInternalService>());
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();

            // Seeders.
            services.AddTransient<Seeder>();
            services.AddTransient<UserSeeder>();
            services.AddTransient<CustomerSeeder>();

            // Security.
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Time.
            services.AddScoped<IClock, Clock>();
            
            // Fluent validation.
            services.AddValidatorsFromAssemblyContaining<UserListRequestDto>(includeInternalTypes: true);
            ValidatorOptions.Global.LanguageManager.Enabled = true;
            ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
            {
                Culture = new("vi")
            };
        }
    }

    extension(IServiceProvider serviceProvider)
    {
        public async Task EnsureDatabaseCreatedAsync()
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
    }

    public static async Task SeedDataAsync(this IServiceProvider serviceProvider, bool isDevelopment)
    {
        IServiceScopeFactory serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
        Seeder seeder = serviceScope.ServiceProvider.GetRequiredService<Seeder>();
        
        await seeder.SeedAsync(isDevelopment);
    }
}