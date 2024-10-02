namespace NATSInternal.Services.Exceptions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Configures and adds the required dependencies which are related to
    /// EntityFrameworkCore, Identity and database handlers into the application's service
    /// collection.
    /// </summary>
    /// <param name="services">
    /// An <see cref="IServiceCollection"/> interface containing the services for the
    /// application.
    /// </param>
    /// <param name="connectionString">
    /// A <see cref="string"/> value representing the connection string for database
    /// connection.
    /// </param>
    /// <returns>
    /// An <see cref="IServiceCollection"/> interface containing the services for the
    /// application.
    /// </returns>
    public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            string connectionString)
    {
        services.AddDbContext<DatabaseContext>(options => options
            .UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                x => x.MigrationsAssembly("NATSInternal.Services"))
            .AddInterceptors(new VietnamTimeInterceptor()));

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddErrorDescriber<VietnameseIdentityErrorDescriber>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 0;
        });

        services.AddScoped<SignInManager<User>>();
        services.AddScoped<RoleManager<Role>>();
        services.AddScoped<DatabaseContext>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<SqlExceptionHandler>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<ISupplyService, SupplyService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ITreatmentService, TreatmentService>();
        services.AddScoped<IDebtIncurrenceService, DebtIncurrenceService>();
        services.AddScoped<IDebtPaymentService, DebtPaymentService>();
        services.AddScoped<IConsultantService, ConsultantService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<INotificationService, NotificationService>();

        services.AddScoped<AuthorizationService>();
        services.AddScoped<IAuthorizationService>(provider => provider
            .GetRequiredService<AuthorizationService>());
        services.AddScoped<IAuthorizationInternalService>(provider => provider
            .GetRequiredService<AuthorizationService>());
            
        services.AddScoped<StatsService>();
        services.AddScoped<IStatsService>(provider => provider
            .GetRequiredService<StatsService>());
        services.AddScoped<IStatsInternalService>(provider => provider
            .GetRequiredService<StatsService>());

        return services;
    }
}