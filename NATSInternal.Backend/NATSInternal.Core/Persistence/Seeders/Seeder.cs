using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Products;
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
    private readonly SupplySeeder _supplySeeder;
    private readonly OrderSeeder _orderSeeder;
    private readonly PaymentSeeder _paymentSeeder;
    private readonly Random _random;
    private readonly ILogger<Seeder> _logger;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public Seeder(
        AppDbContext context,
        UserSeeder userSeeder,
        CustomerSeeder customerSeeder,
        ProductSeeder productSeeder,
        SupplySeeder supplySeeder,
        OrderSeeder orderSeeder,
        PaymentSeeder paymentSeeder,
        ILogger<Seeder> logger,
        IClock clock)
    {
        _context = context;
        _userSeeder = userSeeder;
        _customerSeeder = customerSeeder;
        _productSeeder = productSeeder;
        _supplySeeder = supplySeeder;
        _orderSeeder = orderSeeder;
        _paymentSeeder = paymentSeeder;
        _random = new();
        _logger = logger;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task SeedAsync(bool isDevelopment)
    {
        _logger.LogInformation("Seeding started.");

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        List<User> users = await _userSeeder.SeedAsync(isDevelopment);
        List<Customer> customers = new();

        if (isDevelopment)
        {
            List<Product> products = await _productSeeder.SeedAsync(users);
            async Task<Customer> PickOrSeedCustomerAsync(DateTime generatingDateTime)
            {
                return await _customerSeeder.PickOrSeedSingleCustomerAsync(users, customers, generatingDateTime);
            }

            await SeedFinancialTransactionsAsync(users, PickOrSeedCustomerAsync);
        }

        await transaction.CommitAsync();
        
        _logger.LogInformation("Seeding ended.");
    }
    #endregion

    #region PrivateMethods
    private async Task SeedFinancialTransactionsAsync(
        List<User> users,
        Func<DateTime, Task<Customer>> pickOrSeedCustomerAsync)
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
        
        while (generatingDateTime <= currentDateTime)
        {
            await _supplySeeder.SeedAsync(users, generatingDateTime);
            Order order = await _orderSeeder.SeedSingleOrderAsync(users, generatingDateTime, pickOrSeedCustomerAsync);
            await _paymentSeeder.SeedSinglePaymentAsync(order);
            
            do
            {
                generatingDateTime = generatingDateTime.AddHours(_random.Next(2, 5));
            }
            while (IsDateTimeOutsideBusinessHours(generatingDateTime));
        }
    }

    private static bool IsDateTimeOutsideBusinessHours(DateTime dateTime)
    {
        if (dateTime.Month == 1 && dateTime.Day <= 3 || dateTime.Month == 12 && dateTime.Day >= 28)
        {
            return true;
        }

        switch (dateTime.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                return true;
            case DayOfWeek.Saturday:
                return dateTime.Hour is < 9 or >= 12;
            default:
                return dateTime.Hour is < 9 or >= 18;
        }
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
