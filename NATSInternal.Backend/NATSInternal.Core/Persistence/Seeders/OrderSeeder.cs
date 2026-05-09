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
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Common.Contracts;

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
    public async Task SeedAsync(List<User> users, List<Customer> customers, DateTime generatingDateTime)
    {
        _logger.LogInformation($"Seeding order at dateTime {generatingDateTime:o}.");
        await SeedSingleOrderAsync(users, customers, generatingDateTime);
    }
    #endregion

    #region PrivateMethods
    private async Task SeedSingleOrderAsync(List<User> users, List<Customer> customers, DateTime generatingDateTime)
    {
        OrderType orderType;
        int randomRatio = _random.Next(0, 10);
        if (randomRatio <= 4)
        {
            orderType = OrderType.Treatment;
        }
        else if (randomRatio <= 7)
        {
            orderType = OrderType.Retail;
        }
        else
        {
            orderType = OrderType.Consultant;
        }

        string? note = null;
        if (_random.Next(0, 3) >= 1)
        {
            int sentenceCount = 10;
            do
            {
                note = _viFaker.Lorem.Paragraph(sentenceCount);
                sentenceCount -= 1;
            }
            while (note.Length > HasStatsContracts.NoteMaxLength);
        }

        User createdUser = users
            .Where(u => u.Roles.Any(r => r.Permissions.Any(p => p.Name == PermissionNames.CreateOrder)))
            .OrderBy(_ => Guid.NewGuid())
            .First();

        Customer customer = customers.OrderBy(_ => Guid.NewGuid()).First();

        Order order = new()
        {
            Type = orderType,
            StatsDate = DateOnly.FromDateTime(generatingDateTime),
            Note = note,
            CreatedDateTime = generatingDateTime,
            CreatedUserId = createdUser.Id,
            CustomerId = customer.Id
        };
        
        if (orderType is OrderType.Treatment or OrderType.Retail)
        {
            List<Product> availableProducts = await _context.Products
                .Where(p => p.DeletedDateTime == null && p.StockingQuantity > 0)
                .ToListAsync();

            List<Product> pickedProducts = availableProducts
                .OrderBy(_ => Guid.NewGuid())
                .Take(Math.Min(_random.Next(3, 8), availableProducts.Count))
                .ToList();

            foreach (Product product in pickedProducts)
            {
                double vatRatio = product.DefaultVatPercentagePerUnit / 100;
                OrderProductItem productItem = new()
                {
                    AmountBeforeVatPerUnit = product.DefaultAmountBeforeVatPerUnit,
                    VatAmountPerUnit = (long)Math.Round(product.DefaultAmountBeforeVatPerUnit * vatRatio),
                    Quantity = Math.Min(_random.Next(3, 6), product.StockingQuantity),
                    ProductId = product.Id
                };

                order.ProductItems.Add(productItem);
            }
        }

        if (orderType is OrderType.Treatment or OrderType.Consultant)
        {
            int serviceItemCount = _random.Next(1, 4);
            for (int index = 0; index < serviceItemCount; index += 1)
            {
                string name;
                int wordCount = 10;
                do
                {
                    name = _enFaker.Lorem.Sentence(wordCount, 10);
                    wordCount -= 1;
                }
                while (name.Length > OrderServiceItemContracts.NameMaxLength);

                long amountBeforeVatPerUnit = _random.Next(150, 300) * 1000;
                OrderServiceItem serviceItem = new()
                {
                    Name = name,
                    AmountBeforeVatPerUnit = amountBeforeVatPerUnit,
                    VatAmountPerUnit = (long)Math.Round(amountBeforeVatPerUnit * 0.1),
                    Quantity = _random.Next(1, 4)
                };

                order.ServiceItems.Add(serviceItem);
            }
        }

        order.ComputeCachedProperties();

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
    #endregion
}
