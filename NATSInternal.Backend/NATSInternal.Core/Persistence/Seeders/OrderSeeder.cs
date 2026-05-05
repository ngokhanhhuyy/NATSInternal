using System.Text.RegularExpressions;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal class OrderSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IClock _clock;
    private readonly ILogger<OrderSeeder> _logger;
    private readonly Faker _viFaker;
    private readonly Faker _enFaker;
    private readonly Random _random;
    #endregion
    
    #region Constructors
    public OrderSeeder(AppDbContext context, IClock clock, ILogger<OrderSeeder> logger)
    {
        _context = context;
        _clock = clock;
        _logger = logger;
        _viFaker = new("vi");
        _enFaker = new();
        _random = new();
    }
    #endregion

    #region Methods
    public async Task SeedAsync(List<User> users, List<Customer> customers, bool isDevelopment)
    {
        if (isDevelopment)
        {
            await SeedOrdersAsync(users, customers);
        }
    }
    #endregion

    #region PrivateMethods
    private async Task SeedOrdersAsync(List<User> users, List<Customer> customers)
    {
        if (await _context.Orders.AnyAsync())
        {
            return;
        }

        DateTime currentDateTime = _clock.Now;
        DateTime startingDateTime = _clock.Now.AddMonths(-6);
        DateTime generatingDateTime = startingDateTime;
        
        while (generatingDateTime < _clock.Now)
        {
            
        }
    }
    #endregion
}