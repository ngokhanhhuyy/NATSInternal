using System.Text.Json;
using NATSInternal.Api.Middlewares;
using NATSInternal.Application.Configuration;
using NATSInternal.Infrastructure.Configuration;

namespace NATSInternal.Api;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Connection string - EF Core.
        string connectionString = builder.Configuration.GetConnectionString("Mysql")!;

        // Add services from infrastructure layer.
        string webRootPath = builder.Environment.WebRootPath;
        builder.Services.AddInfrastructureServices(connectionString, webRootPath);

        // Add services from application layer.
        builder.Services.AddApplicationServices();

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

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        
        // Add controllers with json serialization policy.
        builder.Services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            });

        WebApplication app = builder.Build();

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

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseDeveloperExceptionPage();
        app.UseRouting();
        app.UseResponseCaching();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        // app.MapHub<ApplicationHub>("/Api/Hub");
        app.UseStaticFiles();
        app.Run();
    }
}