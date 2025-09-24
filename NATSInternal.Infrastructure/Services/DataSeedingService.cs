using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Core.Constants;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Services;

internal class DataSeedingService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IClock _clock;
    private readonly ILogger<DataSeedingService> _logger;
    #endregion

    #region Constructors
    public DataSeedingService(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        IClock clock,
        ILogger<DataSeedingService> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _clock = clock;
        _logger = logger;
    }
    #endregion

    #region Methods
    public async Task SeedAsync(bool _)
    {
        ICollection<Role> roles = await SeedRolesAsync();
        await SeedUsersAsync(roles);
    }

    private async Task<ICollection<Role>> SeedRolesAsync()
    {
        List<Role> roles = await _context.Roles.ToListAsync();
        if (roles.Count > 0)
        {
            return roles;
        }

        _logger.LogInformation("Seeding roles.");

        roles = new()
        {
            new(RoleNames.Developer, "Nhà phát triển", 50, new List<Permission>()),
            new(RoleNames.Manager, "Quản lý", 40, new List<Permission>()
            {
                new(PermissionNames.CreateUser)
            }),
            new(RoleNames.Accountant, "Kế toán", 30),
            new(RoleNames.Staff, "Nhân viên", 30),
        };

        _context.Roles.AddRange(roles);
        await _context.SaveChangesAsync();

        return roles;
    }

    private async Task<ICollection<User>> SeedUsersAsync(ICollection<Role> roles)
    {
        List<User> users = await _context.Users.ToListAsync();
        if (users.Count > 0)
        {
            return users;
        }

        _logger.LogInformation("Seeding users.");

        users = new() {  };
        Dictionary<string, Role> roleDictionary = roles.ToDictionary(r => r.Name);

        foreach (Role role in roles)
        {
            string userName = $"{role.Name.ToLower()}1";
            User user = new(userName, _passwordHasher.HashPassword(userName), _clock.Now);
            switch (role.Name)
            {
                case RoleNames.Developer:
                    user = new("ngokhanhhuyy", _passwordHasher.HashPassword("Huyy47b1"), _clock.Now);
                    user.AddToRoles(new List<Role>()
                    {
                        roleDictionary[RoleNames.Developer],
                        roleDictionary[RoleNames.Manager],
                        roleDictionary[RoleNames.Accountant],
                        roleDictionary[RoleNames.Staff],
                    });

                    break;
                case RoleNames.Manager:
                    user = new("nguyenthuytrang", _passwordHasher.HashPassword("trang123"), _clock.Now);
                    user.AddToRoles(new List<Role>()
                    {
                        roleDictionary[RoleNames.Manager],
                        roleDictionary[RoleNames.Accountant],
                        roleDictionary[RoleNames.Staff],
                    });

                    break;
                default:
                    user.AddToRoles(new List<Role>() { role });

                    break;
            }

            _context.Users.Add(user);
            _context.Entry(user).Property<string>("NormalizedUserName").CurrentValue = user.UserName.ToLower();
        }

        await _context.SaveChangesAsync();
        return users;
    }
    #endregion
}