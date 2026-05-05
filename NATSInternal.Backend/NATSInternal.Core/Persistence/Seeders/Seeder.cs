using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal class Seeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly UserSeeder _userSeeder;
    private readonly CustomerSeeder _customerSeeder;
    private readonly ProductSeeder _productSeeder;
    private readonly ILogger<Seeder> _logger;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public Seeder(
        AppDbContext context,
        UserSeeder userSeeder,
        CustomerSeeder customerSeeder,
        ProductSeeder productSeeder,
        ILogger<Seeder> logger,
        IClock clock)
    {
        _context = context;
        _userSeeder = userSeeder;
        _customerSeeder = customerSeeder;
        _productSeeder = productSeeder;
        _logger = logger;
    }
    #endregion

    #region Methods
    public async Task SeedAsync(bool isDevelopment)
    {
        _logger.LogInformation("Seeding started.");

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        UserSeededResult userSeededResult = await _userSeeder.SeedAsync(isDevelopment);
        CustomerSeededResult customerSeededResult = await _customerSeeder
            .SeedAsync(userSeededResult.Users, isDevelopment);
        ProductSeededResult productSeededResult = await _productSeeder.SeedAsync(userSeededResult.Users, isDevelopment);
        await transaction.CommitAsync();
        
        _logger.LogInformation("Seeding ended.");
    }
    #endregion

    #region PrivateMethods
    private async Task SeedFinancialTransactionsAsync(List<User> users, List<Customer> customers)
    {
        List<EntityRecordCount> entityRecordCounts = await _context.Supplies
            .Select(s => new EntityRecordCount
            {
                EntityName = nameof(Supply),
                RecordCount = _context.Supplies.Count()
            })
            .Take(1)
            .Union(_context.Orders
                .Select(o => new EntityRecordCount
                {
                    EntityName = nameof(Order),
                    RecordCount = _context.Orders.Count()
                }))
            .ToListAsync();

        if (entityRecordCounts.Any(erc => erc.RecordCount > 0))
        {
            return;
        }

        DateTime currentDateTime = _clock.Now;
        DateTime startingDateTime = _clock.Now.AddMonths(-6);
        DateTime generatingDateTime = startingDateTime;
        
    }
    #endregion
}

file class EntityRecordCount
{
    #region Properties
    public required string EntityName { get; init; }
    public required int RecordCount { get; init; }
    #endregion
}