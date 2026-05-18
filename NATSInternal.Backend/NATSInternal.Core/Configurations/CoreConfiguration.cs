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
using NATSInternal.Core.Features.Expenses;
using NATSInternal.Core.Features.Metadata;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;
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
            services.AddScoped<
                IHasProductService<SupplyUpsertItemRequestDto, SupplyItem>,
                HasProductService<SupplyUpsertItemRequestDto, SupplyItem>>();
            services.AddScoped<
                IHasProductService<OrderProductItemUpsertRequestDto, OrderProductItem>,
                HasProductService<OrderProductItemUpsertRequestDto, OrderProductItem>>();
            services.AddScoped<IStatsMonthYearService, StatsMonthYearService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<AuthorizationInternalService>();
            services.AddScoped<IAuthorizationInternalService>(sp =>
                sp.GetRequiredService<AuthorizationInternalService>());
            services.AddScoped<IAuthorizationService>(sp => sp.GetRequiredService<AuthorizationInternalService>());
            services.AddScoped<CustomerInternalService>();
            services.AddScoped<ICustomerService>(sp => sp.GetRequiredService<CustomerInternalService>());
            services.AddScoped<ICustomerInternalService>(sp => sp.GetRequiredService<CustomerInternalService>());
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IMetadataService, MetadataService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISupplyService, SupplyService>();
            services.AddScoped<PaymentInternalService>();
            services.AddScoped<IPaymentInternalService>(sp => sp.GetRequiredService<PaymentInternalService>());
            services.AddScoped<IPaymentService>(sp => sp.GetRequiredService<PaymentInternalService>());
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            // Seeders.
            services.AddTransient<Seeder>();
            services.AddTransient<CustomerSeeder>();
            services.AddTransient<ProductSeeder>();
            services.AddTransient<SupplySeeder>();
            services.AddTransient<OrderSeeder>();
            services.AddTransient<PaymentSeeder>();
            services.AddTransient<UserSeeder>();

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

        public async Task SeedDataAsync(bool isDevelopment)
        {
            IServiceScopeFactory serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            Seeder seeder = serviceScope.ServiceProvider.GetRequiredService<Seeder>();
        
            await seeder.SeedAsync(isDevelopment);
        }
    }
}
