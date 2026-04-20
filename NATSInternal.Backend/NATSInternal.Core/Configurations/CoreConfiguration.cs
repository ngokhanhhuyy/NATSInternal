using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;
using NATSInternal.Core.Features.Users;

namespace NATSInternal.Core.Configrations;

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
            services.AddScoped<IUserService, UserService>();

            // Security.
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            // Time.
            services.AddScoped<IClock, Clock>();
            
            // Fluent validation.
            services.AddValidatorsFromAssemblyContaining<VerifyUserNameAndPasswordValidator>(includeInternalTypes: true);
            ValidatorOptions.Global.LanguageManager.Enabled = true;
            ValidatorOptions.Global.LanguageManager = new ValidatorLanguageManager
            {
                Culture = new("vi")
            };
        }
    }
}