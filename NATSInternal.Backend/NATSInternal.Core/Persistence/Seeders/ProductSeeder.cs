using System.Text.RegularExpressions;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal partial class ProductSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IClock _clock;
    private readonly ILogger<ProductSeeder> _logger;
    private readonly Faker _viFaker;
    private readonly Faker _enFaker;
    private readonly Random _random;
    #endregion
    
    #region Constrcutors
    public ProductSeeder(AppDbContext context, IClock clock, ILogger<ProductSeeder> logger)
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
    public async Task<ProductSeededResult> SeedAsync(List<User> users, bool isDevelopment)
    {
        if (isDevelopment)
        {
            List<int> userIds = users
                .Where(u => u.Roles.Any(r => r.Permissions.Any(p => p.Name == PermissionNames.CreateProduct)))
                .Select(u => u.Id)
                .ToList();

            foreach (int id in userIds)
            {
                _logger.LogWarning(id.ToString());
            }
            
            List<ProductCategory> categories = await SeedProductCategoriesAsync(userIds);
            List<Product> products = await SeedProductsAsync(userIds, categories);
            await SeedStocksAsync(products);
            
            return new()
            {
                Products = await SeedProductsAsync(userIds, categories),
                ProductCategories = categories 
            };
        }

        return new();
    }
    #endregion
    
    #region PrivateMethods
    private async Task<List<ProductCategory>> SeedProductCategoriesAsync(List<int> _)
    {
        List<ProductCategory> categories = await _context.ProductCategories.ToListAsync();
        if (categories.Count > 0)
        {
            return categories;
        }

        List<string> generatedNames = new();
        _logger.LogInformation("Seeding product categories.");

        for (int index = 0; index < 10; index += 1)
        {
            string name;
            do
            {
                name = _viFaker.Commerce.Categories(1).Single();
            }
            while(generatedNames.Contains(name));
            
            ProductCategory category = new() { Name = name };
            _context.ProductCategories.Add(category);
            categories.Add(category);
            generatedNames.Add(category.Name);
        }

        await _context.SaveChangesAsync();
        
        return categories;
    }

    private async Task<List<Product>> SeedProductsAsync(List<int> userIds, List<ProductCategory> categories)
    {
        List<Product> products = await _context.Products.ToListAsync();
        if (products.Count > 0)
        {
            return products;
        }
        
        _logger.LogInformation("Seeding products.");

        string[] units = new[] { "Chai", "Lọ", "Túi", "Hộp", "Vĩ" };
        List<string> generatedNames = new();

        for (int _ = 0; _ < 30; _++)
        {
            string name = string.Empty;
            do
            {
                name = _enFaker.Commerce.ProductName();
            } while (generatedNames.Contains(name));

            Product product = new()
            {
                
                Name = name,
                Description = _viFaker.Commerce.ProductDescription(),
                Unit = units[_random.Next(units.Length)],
                DefaultAmountBeforeVatPerUnit = _random.Next(200, 1000) * 1000L,
                DefaultVatPercentagePerUnit = 10,
                IsForRetail = _random.Next(10) > 2,
                IsDiscontinued = _random.Next(10) == 0,
                CreatedUserId = userIds.OrderBy(_ => Guid.NewGuid()).First(),
                CreatedDateTime = _clock.Now,
            };

            int categoryCount = Math.Min(_random.Next(0, 4), categories.Count);
            IEnumerable<ProductCategory> pickedCategories = categories
                .OrderBy(_ => Guid.NewGuid())
                .Take(categoryCount);
            
            product.Categories.AddRange(pickedCategories);

            _context.Products.Add(product);
            generatedNames.Add(product.Name);
            products.Add(product);
        }

        await _context.SaveChangesAsync();
        return products;
    }
    
    private async Task SeedStocksAsync(List<Product> products)
    {
        if (await _context.Stocks.AnyAsync())
        {
            return;
        }
        
        _logger.LogInformation("Seeding stocks.");

        foreach (Product product in products)
        {
            Stock stock = new()
            {
                StockingQuantity = _random.Next(20, 300),
                ResupplyThresholdQuantity = _random.Next(5, 10) * 10,
                Product = product
            };

            _context.Stocks.Add(stock);
        }

        await _context.SaveChangesAsync();
    }
    #endregion
    
    #region StaticMethods
    [GeneratedRegex(@"^\+\d+")]
    private static partial Regex GetPhoneNumberCountryCodeRegex();
    #endregion
}

internal class ProductSeededResult
{
    #region Properties
    public List<Product> Products { get; init; } = new();
    public List<ProductCategory> ProductCategories { get; init; } = new();
    #endregion
}
