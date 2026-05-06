using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal class SupplySeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly ILogger<SupplySeeder> _logger;
    private readonly Faker _viFaker;
    private readonly Random _random;
    #endregion
    
    #region Constructors
    public SupplySeeder(AppDbContext context, ILogger<SupplySeeder> logger)
    {
        _context = context;
        _logger = logger;
        _viFaker = new("vi");
        _random = new();
    }
    #endregion

    #region Methods
    public async Task SeedAsync(List<User> users, DateTime generatingDateTime)
    {
        await SeedSingleSupplyAsync(users, generatingDateTime);
    }
    #endregion

    #region PrivateMethods
    private async Task SeedSingleSupplyAsync(List<User> users, DateTime generatingDateTime)
    {
        List<Product> products = await _context.Products
            .Where(p => p.DeletedDateTime == null)
            .Where(p =>
                (p.ResupplyThresholdQuantity == null && p.StockingQuantity <= 5) ||
                (p.ResupplyThresholdQuantity != null && p.StockingQuantity <= p.ResupplyThresholdQuantity))
            .ToListAsync();
            
        if (products.Count == 0)
        {
            return;
        }

        User createdUser = users
            .Where(u => u.Roles.Any(r => r.Permissions.Any(p => p.Name == PermissionNames.CreateSupply)))
            .OrderBy(_ => Guid.NewGuid())
            .First();

        List<SupplyItem> items = new();
        foreach (Product product in products)
        {
            int quantity;
            if (product.ResupplyThresholdQuantity.HasValue)
            {
                quantity = product.ResupplyThresholdQuantity.Value * _random.Next(2, 5);
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

            product.StockingQuantity += quantity;
        }

        string? note = null;
        if (_random.Next(0, 2) == 1)
        {
            int sentenceCount = 10;
            do
            {
                note = _viFaker.Lorem.Paragraph(sentenceCount);
                sentenceCount -= 1;
            }
            while (note.Length >= HasStatsContracts.NoteMaxLength);
        }
        
        Supply supply = new()
        {
            StatsDate = DateOnly.FromDateTime(generatingDateTime),
            ShipmentFee = (int)Math.Round((double)items.Sum(i => i.AmountPerUnit * i.Quantity) / 5),
            Note = note,
            CreatedDateTime = generatingDateTime,
            CreatedUserId = createdUser.Id
        };

        supply.Items.AddRange(items);
        supply.ComputeCachedProperties();

        _context.Supplies.Add(supply);

        await _context.SaveChangesAsync();
    }
    #endregion
}
