using Microsoft.AspNetCore.Identity;
using NATSInternal.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

// Connection string - EF Core.
string connectionString = builder.Configuration.GetConnectionString("Mysql");

// Configure services from Services package/library.
builder.Services.UseCoreServices(connectionString);

// Cookie.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = false;
    options.Cookie.Name = "NATSInternalAuthenticationCookie";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.LoginPath = "/SignIn";
    options.LogoutPath = "/Logout";
    
    options.Events.OnRedirectToLogin = options.Events.OnRedirectToAccessDenied = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToLogout = (context) =>
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    };
});

// Authentication by cookie strategies.
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme);

// Authorization policies.
builder.Services
    .AddAuthorizationBuilder()

    // Users.
    .AddPolicy("CanCreateUser", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateUser))
    .AddPolicy("CanResetPassword", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.ResetOtherUserPassword))
    .AddPolicy("CanDeleteUser", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteUser))
    .AddPolicy("CanRestoreUser", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.RestoreUser))

    // Customers.
    .AddPolicy("CanGetCustomerDetail", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.GetCustomerDetail))
    .AddPolicy("CanCreateCustomer", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateCustomer))
    .AddPolicy("CanEditCustomer", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditCustomer))
    .AddPolicy("CanDeleteCustomer", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteCustomer))

    // Brands.
    .AddPolicy("CanCreateBrand", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateBrand))
    .AddPolicy("CanEditBrand", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditBrand))
    .AddPolicy("CanDeleteBrand", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteBrand))

    // Products.
    .AddPolicy("CanCreateProduct", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateProduct))
    .AddPolicy("CanEditProduct", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditProduct))
    .AddPolicy("CanDeleteProduct", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteProduct))

    // ProductCategory.
    .AddPolicy("CanCreateProductCategory", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateProductCategory))
    .AddPolicy("CanEditProductCategory", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditProductCategory))
    .AddPolicy("CanDeleteProductCategory", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteProductCategory))

    // Supplies.
    .AddPolicy("CanCreateSupply", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateSupply))
    .AddPolicy("CanEditSupply", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditSupply))
    .AddPolicy("CanDeleteSupply", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteSupply))

    // Expenses.
    .AddPolicy("CanCreateExpense", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateExpense))
    .AddPolicy("CanEditExpense", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditExpense))
    .AddPolicy("CanDeleteExpense", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteExpense))

    // Orders.
    .AddPolicy("CanCreateOrder", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateOrder))
    .AddPolicy("CanEditOrder", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditOrder))
    .AddPolicy("CanDeleteOrder", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteOrder))

    // Treatments.
    .AddPolicy("CanCreateTreatment", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateTreatment))
    .AddPolicy("CanEditTreatment", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditTreatment))
    .AddPolicy("CanDeleteTreatment", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteTreatment))

    // DebtIncurrence.
    .AddPolicy("CanCreateDebtIncurrence", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateDebtIncurrence))
    .AddPolicy("CanEditDebtIncurrence", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditDebtIncurrence))
    .AddPolicy("CanDeleteDebtIncurrence", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteDebtIncurrence))

    // DebtPayments.
    .AddPolicy("CanCreateDebtPayment", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateDebtPayment))
    .AddPolicy("CanEditDebtPayment", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditDebtPayment))
    .AddPolicy("CanDeleteDebtPayment", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteDebtPayment))

    // Consultants.
    .AddPolicy("CanCreateConsultant", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateConsultant))
    .AddPolicy("CanEditConsultant", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditConsultant))
    .AddPolicy("CanDeleteConsultant", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteConsultant))

    // Announcements.
    .AddPolicy("CanCreateAnnouncement", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.CreateAnnouncement))
    .AddPolicy("CanEditAnnouncement", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.EditAnnouncement))
    .AddPolicy("CanDeleteAnnouncement", policy =>
        policy.RequireClaim("Permission", PermissionNameConstants.DeleteAnnouncement));

// Add controllers with json serialization policy.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

// Dependency injection
builder.Services.AddScoped<INotifier, Notifier>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
await app.Services.EnsureDatabaseCreatedAsync();
await app.Services.SeedDataAsync();

app.UseCors("LocalhostDevelopment");
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("Origin"))
    {
        string origin = context.Request.Headers.Origin.ToString();
        context.Response.Headers.AccessControlAllowOrigin = origin;
        context.Response.Headers.AccessControlAllowCredentials = "true";
    }

    await next();
}); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ApplicationHub>("/Api/Hub");
app.UseStaticFiles();
app.Run();