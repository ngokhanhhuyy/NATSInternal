using System.Text.RegularExpressions;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal class SupplySeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IClock _clock;
    private readonly ILogger<SupplySeeder> _logger;
    private readonly Faker _viFaker;
    private readonly Faker _enFaker;
    private readonly Random _random;
    #endregion
    
    #region Constructors
    public SupplySeeder(AppDbContext context, IClock clock, ILogger<SupplySeeder> logger)
    {
        
    }
    #endregion

    #region PrivateMethods
    private async Task SeedSingleSupplyAsync(List<User> users, List<Customer> customers, DateTime generatingDateTime)
    {
        List<Product> products = await _context.Products
            .Include(p => p.Stock)
            .Where(p => p.Stock != null && p.Stock.StockingQuantity <= p.Stock.ResupplyThresholdQuantity)
            .ToListAsync();

        List<SupplyItem> items = new();
        foreach (Product product in products)
        {
            int quantity;
            if (product.Stock is not null && product.Stock.ResupplyThresholdQuantity.HasValue)
            {
                quantity = product.Stock.ResupplyThresholdQuantity.Value * _random.Next(2, 5);
            }
            else
            {
                quantity = _random.Next(30, 50);
            }

            SupplyItem item = new()
            {
                AmountPerUnit = (int)Math.Round((double)product.DefaultAmountBeforeVatPerUnit / 3 * 2),
                Quantity = quantity,
                ProductId = product.Id
            };

            product.Stock ??= new();
            product.Stock.StockingQuantity += quantity;
        }
        
        Supply supply = new()
        {
            StatsDate = DateOnly.FromDateTime(generatingDateTime),
            ShipmentFee = s
        }
    }
    #endregion
}