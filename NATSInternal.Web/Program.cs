using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using NATSInternal.Web.Middlewares;
using NATSInternal.Web.Providers;
using NATSInternal.Application.Configuration;
using NATSInternal.Application.Security;
using NATSInternal.Infrastructure.Configuration;

namespace NATSInternal.Web;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Connection string - EF Core.
        string connectionString = builder.Configuration.GetConnectionString("MySql")!;

        // Add services from infrastructure layer.
        string webRootPath = builder.Environment.WebRootPath;
        builder.Services.AddInfrastructureServices(connectionString, webRootPath);

        // Add services from application layer.
        builder.Services.AddApplicationServices();
        
        // Add services from api layer.
        builder.Services.AddScoped<CallerDetailProvider>();
        builder.Services.AddScoped<ICallerDetailProvider>(p => p.GetRequiredService<CallerDetailProvider>());

        // Add MediatR.
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Cookie.
        builder.Services
            .AddAuthentication()
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = false;
                options.Cookie.Name = "NATSInternalAuthenticationCookie";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.LoginPath = "/dang-nhap";
                options.LogoutPath = "/dang-xuat";
            });

        builder.Services.AddAuthorization();
        
        // Add controllers.
        builder.Services
            .AddControllersWithViews(options => options.ModelValidatorProviders.Clear())
            .AddRazorRuntimeCompilation();
        
        // Anti-forgery.
        builder.Services.AddAntiforgery();
        
        // Swagger + OpenAPI.
        builder.Services.AddEndpointsApiExplorer();

        // Build application.
        WebApplication app = builder.Build();
        app.UseForwardedHeaders(new()
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        // Configure database and seed data.
        await app.Services.EnsureDatabaseCreatedAsync();
        await app.Services.SeedDataAsync(app.Environment.IsDevelopment());

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // app.UseHttpsRedirection();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseDeveloperExceptionPage();
        app.UseResponseCaching();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<CallerDetailExtractingMiddleware>();
        app.MapControllers();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Dashboard}/{action=Index}");
        await app.RunAsync();
    }
}