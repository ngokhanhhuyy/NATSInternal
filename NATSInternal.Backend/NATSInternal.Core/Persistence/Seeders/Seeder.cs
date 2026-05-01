using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
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
    #endregion

    #region Constructors
    public Seeder(
        AppDbContext context,
        UserSeeder userSeeder,
        CustomerSeeder customerSeeder,
        ProductSeeder productSeeder,
        ILogger<Seeder> logger)
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
}