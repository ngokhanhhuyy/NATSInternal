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
    /// <param name="isForBlazor">
    /// A <see cref="bool"/> value indicates if this service layer is consumed by Blazor app.
    /// </param>
    /// <returns>
    /// An <see cref="IServiceCollection"/> interface containing the services for the
    /// application.
    /// </returns>
    public static IServiceCollection ConfigureServices(
            this IServiceCollection services,
            string connectionString, // connectionString
            bool isForBlazor = false)
    {
        services.AddDbContextFactory<DatabaseContext>(options => options
            // .UseSqlite(
            //     "DataSource=database.db",
            //     x => x.MigrationsAssembly("NATSInternal.Services")));
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

        services.AddHttpContextAccessor();

        services.AddScoped<SignInManager<User>>();
        services.AddScoped<RoleManager<Role>>();

        if (isForBlazor)
        {
            services.AddTransient<DatabaseContext>();
            services.AddTransient<IUserService, UserService>();
        }
        else
        {
            services.AddScoped<DatabaseContext>();
            services.AddScoped<IUserService, UserService>();
        }
        
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRoleService, RoleService>();

        // Photo services.
        services.AddScoped<ISinglePhotoService<Brand>, SinglePhotoService<Brand>>();
        services.AddScoped<
            IMultiplePhotosService<Supply, SupplyPhoto>,
            MultiplePhotosService<Supply, SupplyPhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Expense, ExpensePhoto>,
            MultiplePhotosService<Expense, ExpensePhoto>>();
        services.AddScoped<
            IMultiplePhotosService<Order, OrderPhoto>,
            MultiplePhotosService<Order, OrderPhoto>>();

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
        services.AddScoped<ISinglePhotoService<User>, SinglePhotoService<User>>();
        services.AddScoped<ISinglePhotoService<Brand>, SinglePhotoService<Brand>>();
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
            IMultiplePhotosService<Order, OrderPhoto>,
            MultiplePhotosService<Order, OrderPhoto>>();
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
            IMonthYearService<Order, OrderUpdateHistory>,
            MonthYearService<Order, OrderUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Treatment, TreatmentUpdateHistory>,
            MonthYearService<Treatment, TreatmentUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<Consultant, ConsultantUpdateHistory>,
            MonthYearService<Consultant, ConsultantUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<DebtIncurrence, DebtIncurrenceUpdateHistory>,
            MonthYearService<DebtIncurrence, DebtIncurrenceUpdateHistory>>();
        services.AddScoped<
            IMonthYearService<DebtPayment, DebtPaymentUpdateHistory>,
            MonthYearService<DebtPayment, DebtPaymentUpdateHistory>>();

        return services;
    }
}