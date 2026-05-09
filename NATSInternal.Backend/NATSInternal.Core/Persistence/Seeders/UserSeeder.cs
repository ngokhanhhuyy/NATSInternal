using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

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
    public async Task<UserSeededResult> SeedAsync(bool isDevelopment)
    {
        List<Role> roles = await SeedRolesAsync();
        List<User> users = await SeedUsersAsync(roles, isDevelopment);

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
            new()
            {
                Name = RoleNames.Developer,
                DisplayName = "Nhà phát triển",
                PowerLevel = 50,
                Permissions = new()
                {
                    new() { Name = PermissionNames.GetApplicationOperationReport },
                }
            },
            new()
            {
                Name = RoleNames.Manager,
                DisplayName = "Quản lý",
                PowerLevel = 40,
                Permissions = new()
                {
                    new() { Name = PermissionNames.CreateUser },
                    new() { Name = PermissionNames.UpdateAnotherUser },
                    new() { Name = PermissionNames.ResetAnotherUserPassword },
                    new() { Name = PermissionNames.DeleteAnotherUser },
                    new() { Name = PermissionNames.CreateAnnouncement },
                    new() { Name = PermissionNames.EditAnnouncement },
                    new() { Name = PermissionNames.DeleteAnnouncement },
                }
            },
            new()
            {
                Name = RoleNames.Accountant,
                DisplayName = "Kế toán",
                PowerLevel = 30,
                Permissions = new()
                {
                    new() { Name = PermissionNames.EditCustomer },
                    new() { Name = PermissionNames.DeleteCustomer },
                    new() { Name = PermissionNames.CreateProduct },
                    new() { Name = PermissionNames.EditProduct },
                    new() { Name = PermissionNames.DeleteProduct },
                    new() { Name = PermissionNames.CreateProductCategory },
                    new() { Name = PermissionNames.EditProductCategory },
                    new() { Name = PermissionNames.DeleteProductCategory },
                    new() { Name = PermissionNames.EditSupply },
                    new() { Name = PermissionNames.DeleteSupply },
                    new() { Name = PermissionNames.EditExpense },
                    new() { Name = PermissionNames.DeleteExpense },
                    new() { Name = PermissionNames.EditOrder },
                    new() { Name = PermissionNames.DeleteOrder },
                    new() { Name = PermissionNames.EditDebt },
                    new() { Name = PermissionNames.DeleteDebt },
                    new() { Name = PermissionNames.GetFinancialReport }
                },
            },
            new()
            {
                Name = RoleNames.Staff,
                DisplayName = "Nhân viên",
                PowerLevel = 30,
                Permissions = new()
                {
                    new() { Name = PermissionNames.CreateCustomer },
                    new() { Name = PermissionNames.CreateSupply },
                    new() { Name = PermissionNames.CreateExpense },
                    new() { Name = PermissionNames.CreateOrder },
                    new() { Name = PermissionNames.CreateDebt },
                },
            }
        };

        _context.Roles.AddRange(roles);
        await _context.SaveChangesAsync();

        return roles;
    }

    private async Task<List<User>> SeedUsersAsync(ICollection<Role> roles, bool isDevelopment)
    {
        List<User> users = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .AsSplitQuery()
            .ToListAsync();
            
        if (users.Count > 0)
        {
            return users;
        }

        _logger.LogInformation("Seeding users.");

        users = new();
        Dictionary<string, Role> roleDictionary = roles.ToDictionary(r => r.Name);

        User huyUser = new()
        {
            UserName = "ngokhanhhuyy",
            PasswordHash = _passwordHasher.HashPassword("Huyy47b1"),
            CreatedDateTime = _clock.Now,
            CreatedUserId = null,
            Roles = new()
            {
                roleDictionary[RoleNames.Developer],
                roleDictionary[RoleNames.Manager],
                roleDictionary[RoleNames.Accountant],
                roleDictionary[RoleNames.Staff],
            },
        };
        _context.Users.Add(huyUser);
        users.Add(huyUser);

        User trangUser = new()
        {
            UserName = "nguyenthuytrang",
            PasswordHash = _passwordHasher.HashPassword("trang123"),
            CreatedDateTime = _clock.Now,
            CreatedUserId = null,
            Roles = new()
            {
                roleDictionary[RoleNames.Manager],
                roleDictionary[RoleNames.Accountant],
                roleDictionary[RoleNames.Staff],
            }
        };
        _context.Users.Add(trangUser);
        users.Add(trangUser);

        await _context.SaveChangesAsync();

        if (isDevelopment)
        {
            foreach (Role role in roles)
            {
                int createdUserId = 1;
                if (users.Count > 0)
                {
                    createdUserId = users
                        .Where(u => u.Roles.Select(r => r.Name).Contains(RoleNames.Developer))
                        .OrderBy(_ => Guid.NewGuid())
                        .Select(u => u.Id)
                        .First();
                };

                string userName = $"{role.Name.ToLower()}1";
                User user = new()
                {
                    UserName = userName,
                    PasswordHash = _passwordHasher.HashPassword(userName),
                    CreatedDateTime = _clock.Now,
                    CreatedUserId = huyUser.Id,
                    Roles = new() { role }
                };

                _context.Users.Add(user);
                users.Add(user);
            }
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