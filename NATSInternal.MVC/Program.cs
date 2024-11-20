using NATSInternal.Middlewares;
using System.Security.Claims;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("~/Views/{1}/{0}View.cshtml");
        options.PageViewLocationFormats.Add("~/Views/{1}/{0}Partial.cshtml");
    }).AddRazorRuntimeCompilation();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});

// Connection string - EF Core.
string connectionString = builder.Configuration.GetConnectionString("Mysql");

// Configure services from Services package/library.
builder.Services.ConfigureServices(connectionString);

// Cookie.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
    options.Cookie.Name = "NATSInternalAuthenticationCookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/SignIn";
    options.LogoutPath = "/SignOut";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Authentication by cookie strategies.
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme);

// Authorization policies.
builder.Services
    .AddAuthorizationBuilder()

    // Users.
    .AddPolicy("CanCreateUser", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateUser))
    .AddPolicy("CanResetPassword", policy =>
        policy.RequireClaim("Permission", PermissionConstants.ResetOtherUserPassword))
    .AddPolicy("CanDeleteUser", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteUser))
    .AddPolicy("CanRestoreUser", policy =>
        policy.RequireClaim("Permission", PermissionConstants.RestoreUser))

    // Customers.
    .AddPolicy("CanGetCustomerDetail", policy =>
        policy.RequireClaim("Permission", PermissionConstants.GetCustomerDetail))
    .AddPolicy("CanCreateCustomer", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateCustomer))
    .AddPolicy("CanEditCustomer", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditCustomer))
    .AddPolicy("CanDeleteCustomer", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteCustomer))

    // Brands.
    .AddPolicy("CanCreateBrand", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateBrand))
    .AddPolicy("CanEditBrand", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditBrand))
    .AddPolicy("CanDeleteBrand", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteBrand))

    // Products.
    .AddPolicy("CanCreateProduct", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateProduct))
    .AddPolicy("CanEditProduct", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditProduct))
    .AddPolicy("CanDeleteProduct", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteProduct))

    // ProductCategory.
    .AddPolicy("CanCreateProductCategory", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateProductCategory))
    .AddPolicy("CanEditProductCategory", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditProductCategory))
    .AddPolicy("CanDeleteProductCategory", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteProductCategory))

    // Supplies.
    .AddPolicy("CanCreateSupply", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateSupply))
    .AddPolicy("CanEditSupply", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditSupply))
    .AddPolicy("CanDeleteSupply", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteSupply))

    // Expenses.
    .AddPolicy("CanCreateExpense", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateExpense))
    .AddPolicy("CanEditExpense", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditExpense))
    .AddPolicy("CanDeleteExpense", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteExpense))

    // Orders.
    .AddPolicy("CanCreateOrder", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateOrder))
    .AddPolicy("CanEditOrder", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditOrder))
    .AddPolicy("CanDeleteOrder", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteOrder))

    // Treatments.
    .AddPolicy("CanCreateTreatment", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateTreatment))
    .AddPolicy("CanEditTreatment", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditTreatment))
    .AddPolicy("CanDeleteTreatment", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteTreatment))

    // DebtIncurrence.
    .AddPolicy("CanCreateDebtIncurrence", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateDebtIncurrence))
    .AddPolicy("CanEditDebtIncurrence", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditDebtIncurrence))
    .AddPolicy("CanDeleteDebtIncurrence", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteDebtIncurrence))

    // DebtPayments.
    .AddPolicy("CanCreateDebtPayment", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateDebtPayment))
    .AddPolicy("CanEditDebtPayment", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditDebtPayment))
    .AddPolicy("CanDeleteDebtPayment", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteDebtPayment))

    // Consultants.
    .AddPolicy("CanCreateConsultant", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateConsultant))
    .AddPolicy("CanEditConsultant", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditConsultant))
    .AddPolicy("CanDeleteConsultant", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteConsultant))

    // Announcements.
    .AddPolicy("CanCreateAnnouncement", policy =>
        policy.RequireClaim("Permission", PermissionConstants.CreateAnnouncement))
    .AddPolicy("CanEditAnnouncement", policy =>
        policy.RequireClaim("Permission", PermissionConstants.EditAnnouncement))
    .AddPolicy("CanDeleteAnnouncement", policy =>
        policy.RequireClaim("Permission", PermissionConstants.DeleteAnnouncement));


// FluentValidations
builder.Services.ConfigureFluentValidation();

// Add controllers with json serialization policy.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

// Dependancy injections.
builder.Services.AddScoped<INotifier, Notifier>();
builder.Services.AddHttpClient();

// Add CORS.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "LocalhostDevelopment",
        policyBuilder => policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod());
});

WebApplication app = builder.Build();
DataInitializer dataInitializer;
dataInitializer = new DataInitializer();
dataInitializer.InitializeData(app);

app.UseCors("LocalhostDevelopment");
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("Origin"))
    {
        string origin = context.Request.Headers["Origin"].ToString();
        context.Response.Headers["Access-Control-Allow-Origin"] = origin;
        context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
    }

    await next();
}); 

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    app.UseHttpsRedirection();
}
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.MapHub<WrapperHub>("/Wrapper");
app.UseStaticFiles();
app.Run();