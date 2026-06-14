using Microsoft.Extensions.Logging;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal class PaymentSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly Random _random = new();
    private readonly ILogger<OrderSeeder> _logger;
    #endregion
    
    #region Constructors
    public PaymentSeeder(AppDbContext context, ILogger<OrderSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }
    #endregion

    #region Methods
    public async Task SeedSinglePaymentAsync(Order order)
    {
        _logger.LogInformation($"Seeding payment at dateTime {order.CreatedDateTime:o}.");

        int debtAndRefundChance = _random.Next(0, 200);
        long amount = order.AmountAfterVat;
        if (debtAndRefundChance < 4)
        {
            amount -= (long)Math.Ceiling(amount * 0.1M / 1000M) * 1000;
        }
        else if (debtAndRefundChance == 199)
        {
            amount += (long)Math.Ceiling(amount * 0.15M / 1000M) * 1000;
        }

        Payment payment = new()
        {
            StatsDate = order.StatsDate,
            Type = PaymentType.OrderPayment,
            Amount = amount,
            CustomerId = order.CustomerId,
            OrderId = order.Id,
            CreatedDateTime = order.CreatedDateTime,
            CreatedUserId = order.CreatedUserId,
        };

        order.Customer.CachedDebtAmount += order.AmountAfterVat - payment.Amount;

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();
    }
    #endregion
}
