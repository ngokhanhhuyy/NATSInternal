using Bogus;
using Microsoft.EntityFrameworkCore;
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
    private readonly Faker _viFaker;
    private readonly Random _random;
    #endregion
    
    #region Constructors
    public SupplySeeder(AppDbContext context)
    {
        _context = context;
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
            .Where(p => p.StockingQuantity <= p.ResupplyThresholdQuantity)
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
            if (product.ResupplyThresholdQuantity > 0)
            {
                quantity = product.ResupplyThresholdQuantity * _random.Next(2, 5);
            }
            else
            {
                quantity = _random.Next(30, 50);
            }

            SupplyItem item = new()
            {
                AmountPerUnit = (int)Math.Round(product.DefaultAmountBeforeVatPerUnit * (2M / 3M) / 1000M) * 1000,
                Quantity = quantity,
                ProductId = product.Id
            };

            product.StockingQuantity += quantity;
            items.Add(item);
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
            ShipmentFee = (int)Math.Round(items.Sum(i => i.AmountPerUnit * i.Quantity) * 0.01M / 1000) * 1000,
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
