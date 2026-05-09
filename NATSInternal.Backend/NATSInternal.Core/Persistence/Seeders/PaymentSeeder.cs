using Microsoft.Extensions.Logging;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Persistence.Seeders;

internal class PaymentSeeder
{
    #region Fields
    private readonly AppDbContext _context;
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

        Payment payment = new()
        {
            StatsDate = order.StatsDate,
            Type = PaymentType.OrderPayment,
            Amount = order.CachedAmountAfterVat,
            CustomerId = order.CustomerId,
            OrderId = order.Id,
            CreatedDateTime = order.CreatedDateTime,
            CreatedUserId = order.CreatedUserId,
        };

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();
    }
    #endregion
}
