using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Seeders;

internal class UserSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IClock _clock;
    private readonly ILogger<UserSeeder> _logger;
    #endregion

    #region Constructors
    public UserSeeder(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        IClock clock,
        ILogger<UserSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _clock = clock;
        _logger = logger;
    }
    #endregion

    #region Methods
    public async Task<UserSeededResult> SeedAsync()
    {
        
        List<Role> roles = await SeedRolesAsync();
        List<User> users = await SeedUsersAsync(roles);

        return new()
        {
            Users = users,
            Roles = roles
        };
    }
    #endregion
    
    #region PrivateMethods
    private async Task<List<Role>> SeedRolesAsync()
    {
        List<Role> roles = await _context.Roles.ToListAsync();
        if (roles.Count > 0)
        {
            return roles;
        }

        _logger.LogInformation("Seeding roles.");

        roles = new()
        {
            new(RoleNames.Developer, "Nhà phát triển", 50, new List<string>()),
            new(RoleNames.Manager, "Quản lý", 40, new List<string>()
            {
                PermissionNames.CreateUser,
                PermissionNames.AddUserToOrRemoveUserFromRoles,
                PermissionNames.ResetOtherUserPassword,
                PermissionNames.DeleteUser
            }),
            new(RoleNames.Accountant, "Kế toán", 30, new List<string>()),
            new(RoleNames.Staff, "Nhân viên", 30, new List<string>()),
        };

        _context.Roles.AddRange(roles);
        await _context.SaveChangesAsync();

        return roles;
    }

    private async Task<List<User>> SeedUsersAsync(ICollection<Role> roles)
    {
        List<User> users = await _context.Users.ToListAsync();
        if (users.Count > 0)
        {
            return users;
        }

        _logger.LogInformation("Seeding users.");

        users = new();
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
                    user.AddToRoles(new List<Role> { role });
                    break;
            }

            _context.Users.Add(user);
            users.Add(user);
        }

        await _context.SaveChangesAsync();
        return users;
    }
    #endregion
}

    
#region Classes
internal class UserSeededResult
{
    #region Properties
    public required List<User> Users { get; init; }
    public required List<Role> Roles { get; init; }
    #endregion
}
#endregion