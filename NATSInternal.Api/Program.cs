using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using NATSInternal.Api.Middlewares;
using NATSInternal.Api.Providers;
using NATSInternal.Api.Filters;
using NATSInternal.Application.Configuration;
using NATSInternal.Application.Notification;
using NATSInternal.Application.Security;
using NATSInternal.Infrastructure.Configuration;

namespace NATSInternal.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        string contentRoot = AppContext.BaseDirectory;
        WebApplicationBuilder builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = contentRoot,
            WebRootPath = Path.Combine(contentRoot, "wwwroot")
        });
        
        builder.Configuration
            .SetBasePath(contentRoot)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

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
        builder.Services.AddScoped<INotificationService, NotificationService>();

        // Add MediatR.
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Cookie.
        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = false;
                options.Cookie.Name = "NATSInternalAuthenticationCookie";
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.LoginPath = "/SignIn";
                options.LogoutPath = "/Logout";

                // options.Events.OnSignedIn = context =>
                // {
                //     CallerDetailProvider callerDetailProvider = context.HttpContext.RequestServices
                //         .GetRequiredService<CallerDetailProvider>();
                //     try
                //     {
                //         callerDetailProvider.SetCallerDetail(context.Principal!);
                //     }
                //     catch (AuthenticationException exception)
                //     {
                //         context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //         return context.Fail(exception);
                //     }
                //
                //     return Task.CompletedTask;
                // };
                
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
        builder.Services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<ProblemDetailsSchemaFilter>();
            options.OperationFilter<RemoveSchemaForNotFoundOperationFilter>();
        });
        
        // Add controllers with json serialization policy.
        builder.Services
            .AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            });
        
        // Swagger + OpenAPI.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Build application.
        WebApplication app = builder.Build();

        // Configure database and seed data.
        await app.Services.EnsureDatabaseCreatedAsync();
        await app.Services.SeedDataAsync(app.Environment.IsDevelopment());
        
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
        app.UseMiddleware<CallerDetailExtractingMiddleware>();
        app.MapControllers();
        // app.MapHub<ApplicationHub>("/Api/Hub");
        app.UseStaticFiles();
        await app.RunAsync();
    }
}