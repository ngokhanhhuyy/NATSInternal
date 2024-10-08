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

        // Photo services.
        services.AddScoped<IPhotoService<Brand>, PhotoService<Brand>>();
        services.AddScoped<
            IPhotoService<Supply, SupplyPhoto>,
            PhotoService<Supply, SupplyPhoto>>();
        services.AddScoped<
            IPhotoService<Expense, ExpensePhoto>,
            PhotoService<Expense, ExpensePhoto>>();
        services.AddScoped<
            IPhotoService<Order, OrderPhoto>,
            PhotoService<Order, OrderPhoto>>();

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
            
        // Stats services.
        services.AddScoped<StatsService>();
        services.AddScoped<IStatsService>(provider => provider
            .GetRequiredService<StatsService>());
        services.AddScoped<
            IStatsInternalService<Expense, User, ExpenseUpdateHistory>,
            StatsInternalService<Expense, User, ExpenseUpdateHistory>>();
        services.AddScoped<
            IStatsInternalService<Supply, User, SupplyUpdateHistory>,
            StatsInternalService<Supply, User, SupplyUpdateHistory>>();
        services.AddScoped<
            IStatsInternalService<Order, User, OrderUpdateHistory>,
            StatsInternalService<Order, User, OrderUpdateHistory>>();
        services.AddScoped<
            IStatsInternalService<Treatment, User, TreatmentUpdateHistory>,
            StatsInternalService<Treatment, User, TreatmentUpdateHistory>>();
        services.AddScoped<
            IStatsInternalService<Consultant, User, ConsultantUpdateHistory>,
            StatsInternalService<Consultant, User, ConsultantUpdateHistory>>();
        services.AddScoped<
            IStatsInternalService<DebtIncurrence, User, DebtIncurrenceUpdateHistory>,
            StatsInternalService<DebtIncurrence, User, DebtIncurrenceUpdateHistory>>();
        services.AddScoped<
            IStatsInternalService<DebtPayment, User, DebtPaymentUpdateHistory>,
            StatsInternalService<DebtPayment, User, DebtPaymentUpdateHistory>>();

        // Product-engagement services.
        services.AddScoped<
            IProductEngagementService<SupplyItem, Product, SupplyPhoto, User, SupplyUpdateHistory>,
            ProductEngagementService<SupplyItem, SupplyPhoto, SupplyUpdateHistory>>();
        services.AddScoped<
            IProductEngagementService<OrderItem, Product, OrderPhoto, User, OrderUpdateHistory>,
            ProductEngagementService<OrderItem, OrderPhoto, OrderUpdateHistory>>();
        services.AddScoped<
            IProductEngagementService<TreatmentItem, Product, TreatmentPhoto, User, TreatmentUpdateHistory>,
            ProductEngagementService<TreatmentItem, TreatmentPhoto, TreatmentUpdateHistory>>();

        // Month-year services.
        services.AddScoped<
            IMonthYearService<Expense, User, ExpenseUpdateHistory>,
            MonthYearService<Expense, User, ExpenseUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Supply, User, SupplyUpdateHistory>,
            MonthYearService<Supply, User, SupplyUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Order, User, OrderUpdateHistory>,
            MonthYearService<Order, User, OrderUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Treatment, User, TreatmentUpdateHistory>,
            MonthYearService<Treatment, User, TreatmentUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Consultant, User, ConsultantUpdateHistory>,
            MonthYearService<Consultant, User, ConsultantUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<DebtIncurrence, User, DebtIncurrenceUpdateHistory>,
            MonthYearService<DebtIncurrence, User, DebtIncurrenceUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<DebtPayment, User, DebtPaymentUpdateHistory>,
            MonthYearService<DebtPayment, User, DebtPaymentUpdateHistory>>();

        return services;
    }
}