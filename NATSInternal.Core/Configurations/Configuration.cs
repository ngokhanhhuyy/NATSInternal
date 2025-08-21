namespace NATSInternal.Core.Exceptions;

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
    public static IServiceCollection UseCoreServices(
            this IServiceCollection services,
            string connectionString)
    {
        services.AddDbContextFactory<DatabaseContext>(options => options
            .UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                x => x.MigrationsAssembly("NATSInternal.Core"))
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

        services.AddHttpContextAccessor();

        services.AddScoped<DatabaseContext>();
        services.AddScoped<SignInManager<User>>();
        services.AddScoped<RoleManager<Role>>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRoleService, RoleService>();

        // Photo services.
        services.AddScoped<IPhotoService<Brand>, PhotoService<Brand>>();
        services.AddScoped<
            IMultiplePhotosService<Supply, SupplyPhoto>,
            MultiplePhotosService<Supply, SupplyPhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Expense, ExpensePhoto>,
            MultiplePhotosService<Expense, ExpensePhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Order, Photo>,
            MultiplePhotosService<Order, Photo>>();

        services.AddScoped<ITreatmentPhotoService, TreatmentPhotoService>();
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

        // Authorization.
        services.AddScoped<AuthorizationService>();
        services.AddScoped<IAuthorizationService>(provider => provider
            .GetRequiredService<AuthorizationService>());
        services.AddScoped<IAuthorizationInternalService>(provider => provider
            .GetRequiredService<AuthorizationService>());

        // Photo services.
        services.AddScoped<IPhotoService<User>, PhotoService<User>>();
        services.AddScoped<IPhotoService<Brand>, PhotoService<Brand>>();
        services.AddScoped<
            IMultiplePhotosService<Product, ProductPhoto>,
            MultiplePhotosService<Product, ProductPhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Expense, ExpensePhoto>,
            MultiplePhotosService<Expense, ExpensePhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Supply, SupplyPhoto>,
            MultiplePhotosService<Supply, SupplyPhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Order, Photo>,
            MultiplePhotosService<Order, Photo>>();
        services.AddScoped<
            IMultiplePhotosService<Treatment, TreatmentPhoto>,
            MultiplePhotosService<Treatment, TreatmentPhoto>>();

        // Stats services.
        services.AddScoped<StatsService>();
        services.AddScoped<IStatsService>(provider => provider
            .GetRequiredService<StatsService>());
        services.AddScoped<IStatsInternalService, StatsInternalService>();

        // Month-year services.
        services.AddScoped<
            IMonthYearService<Expense, ExpenseUpdateHistory>,
            MonthYearService<Expense, ExpenseUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Supply, SupplyUpdateHistory>,
            MonthYearService<Supply, SupplyUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Order, UpdateHistory>,
            MonthYearService<Order, UpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Treatment, TreatmentUpdateHistory>,
            MonthYearService<Treatment, TreatmentUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Consultant, ConsultantUpdateHistory>,
            MonthYearService<Consultant, ConsultantUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Debt, DebtUpdateHistory>,
            MonthYearService<Debt, DebtUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<DebtPayment, DebtPaymentUpdateHistory>,
            MonthYearService<DebtPayment, DebtPaymentUpdateHistory>>();

        // Data seeding.
        services.AddScoped<DataSeedingService>();
        
        // Fluent validation.
        services.AddValidatorsFromAssemblyContaining<SignInValidator>();
        ValidatorOptions.Global.LanguageManager.Enabled = true;
        ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
        {
            Culture = new System.Globalization.CultureInfo("vi")
        };
        ValidatorOptions.Global.PropertyNameResolver = (_, b, _) => b.Name
            .First()
            .ToString()
            .ToLower() + b.Name[1..];

        return services;
    }

    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
    {
        IServiceScopeFactory serviceScopeFactory = serviceProvider
            .GetRequiredService<IServiceScopeFactory>();
        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
        DatabaseContext context = serviceScope.ServiceProvider
            .GetService<DatabaseContext>()
            ?? throw new InvalidOperationException(
                $"{nameof(DatabaseContext)} dependency cannot be resolved.");

        await context.Database.OpenConnectionAsync();
        await context.Database.EnsureCreatedAsync();
        await context.Database.CloseConnectionAsync();
    }

    public static async Task SeedDataAsync(this IServiceProvider serviceProvider)
    {
        IServiceScopeFactory serviceScopeFactory = serviceProvider
            .GetRequiredService<IServiceScopeFactory>();
        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();
        DataSeedingService service = serviceScope.ServiceProvider
            .GetService<DataSeedingService>()
            ?? throw new InvalidOperationException(
                $"{nameof(DataSeedingService)} dependency cannot be resolved.");
        
        await service.SeedAsync();
    }
}