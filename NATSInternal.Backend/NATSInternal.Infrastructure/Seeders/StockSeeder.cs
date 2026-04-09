using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Seeders;

internal class StockSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly ILogger<StockSeeder> _logger;
    private readonly Random _random;
    #endregion
    
    #region Constructors
    public StockSeeder(AppDbContext context, ILogger<StockSeeder> logger)
    {
        _context = context;
        _logger = logger;
        _random = new();
    }
    #endregion
    
    #region Methods
    public async Task SeedAsync(List<Product> products)
    {
        await SeedStocksAsync(products);
    }
    #endregion
    
    #region PrivateMethods
    private async Task SeedStocksAsync(List<Product> products)
    {
        List<Stock> stocks = await _context.Stocks.ToListAsync();
        if (stocks.Count > 0)
        {
            return;
        }
        
        _logger.LogInformation("Seeding stocks.");
        

        foreach (Product product in products)
        {
            Stock stock = new(_random.Next(20, 300), _random.Next(5, 10) * 10, product.Id);
            stocks.Add(stock);
            _context.Stocks.Add(stock);
        }

        await _context.SaveChangesAsync();
    }
    #endregion
}