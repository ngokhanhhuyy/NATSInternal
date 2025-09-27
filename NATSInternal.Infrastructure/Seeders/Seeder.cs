using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Seeders;

internal class Seeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly UserSeeder _userSeeder;
    private readonly ProductSeeder _productSeeder;
    private readonly StockSeeder _stockSeeder;
    private readonly ILogger<Seeder> _logger;
    #endregion

    #region Constructors
    public Seeder(
        AppDbContext context,
        UserSeeder userSeeder,
        ProductSeeder productSeeder,
        StockSeeder stockSeeder,
        ILogger<Seeder> logger)
    {
        _context = context;
        _userSeeder = userSeeder;
        _productSeeder = productSeeder;
        _stockSeeder = stockSeeder;
        _logger = logger;
    }
    #endregion

    #region Methods
    public async Task SeedAsync(bool isDevelopment)
    {
        _logger.LogInformation("Seeding started.");

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        UserSeededResult userSeededResult = await _userSeeder.SeedAsync();
        ProductSeededResult productSeededResult = await _productSeeder.SeedAsync(isDevelopment);
        await _stockSeeder.SeedAsync(productSeededResult.Products);
        await transaction.CommitAsync();
        
        _logger.LogInformation("Seeding ended.");
    }
    #endregion
}