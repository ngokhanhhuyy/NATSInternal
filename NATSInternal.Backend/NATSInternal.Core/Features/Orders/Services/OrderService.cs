using JetBrains.Annotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Payments;
// using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;
using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Orders;

[UsedImplicitly]
internal class OrderService : IOrderService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly ICustomerInternalService _customerService;
    private readonly IPaymentInternalService _paymentService;
    private readonly IListFetchingService _listFetchingService;
    private readonly IStatsMonthYearService _statsMonthYearService;
    private readonly IHasProductService<OrderProductItemUpsertRequestDto, OrderProductItem> _hasProductService;
    private readonly ITopAndCountService _topAndCountService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<OrderListRequestDto> _listValidator;
    private readonly IValidator<OrderUpsertRequestDto> _upsertValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public OrderService(
        AppDbContext context,
        ICustomerInternalService customerService,
        IPaymentInternalService paymentService,
        IListFetchingService listFetchingService,
        IStatsMonthYearService statsMonthYearService,
        IHasProductService<OrderProductItemUpsertRequestDto, OrderProductItem> hasProductService,
        ITopAndCountService topAndCountService,
        IAuthorizationInternalService authorizationService,
        IValidator<OrderListRequestDto> listValidator,
        IValidator<OrderUpsertRequestDto> upsertValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _customerService = customerService;
        _paymentService = paymentService;
        _listFetchingService = listFetchingService;
        _statsMonthYearService = statsMonthYearService;
        _hasProductService = hasProductService;
        _topAndCountService = topAndCountService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
        _exceptionHandler = exceptionHandler;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<OrderListResponseDto> GetListAsync(OrderListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Order> query = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Payments)
            .Include(o => o.Photos.Where(photo => photo.IsThumbnail))
            .Where(o => o.DeletedDateTime == null);

        query = query.HasStatsMonthYear(requestDto.StatsYear, requestDto.StatsMonth);

        if (requestDto.Type.HasValue)
        {
            query = query.Where(o => o.Type == requestDto.Type.Value);
        }
        
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(o => o.CustomerId == requestDto.CustomerId.Value);
        }

        if (requestDto.ProductId.HasValue)
        {
            query = query
                .Include(o => o.ProductItems).ThenInclude(opi => opi.Product)
                .Where(o => o.ProductItems.Any(opi => opi.Product.Id == requestDto.ProductId.Value));
        }

        switch (requestDto.PaymentStatus)
        {
            case OrderListRequestDto.OrderPaymentStatus.Debt:
                query = query.Where(o =>
                    o.Payments.SingleOrDefault(p => p.DeletedDateTime == null) != null ||
                    o.CachedAmountAfterVat > o.Payments.Single(p => p.DeletedDateTime == null).Amount);
                break;
            case OrderListRequestDto.OrderPaymentStatus.RefundNeeded:
                query = query.Where(o =>
                    o.Payments.SingleOrDefault(p => p.DeletedDateTime == null) != null &&
                    o.CachedAmountAfterVat < o.Payments.Single(p => p.DeletedDateTime == null).Amount);
                break;
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(OrderListRequestDto.FieldToSort.StatsDate):
                query = query
                    .ApplySorting(o => o.StatsDate, requestDto.SortByAscending)
                    .ThenApplySorting(o => o.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.CreatedDateTime):
                query = query
                    .ApplySorting(o => o.CreatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(o => o.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.LastUpdatedDateTime):
                query = query
                    .ApplySorting(o => o.LastUpdatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(o => o.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.Amount):
                query = query
                    .ApplySorting(o => o.CachedAmountAfterVat, requestDto.SortByAscending)
                    .ThenApplySorting(o => o.StatsDate, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Order> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<OrderBasicResponseDto> orderResponseDtos = queryResult.Items
            .Select(o => new OrderBasicResponseDto(o, _authorizationService.GetOrderExistingAuthorization(o)))
            .ToList();

        return new(orderResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }

    public async Task<OrderDetailResponseDto> GetDetailAsync(int id)
    {
        Order order = await _context.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.Customer)
            .Include(o => o.ProductItems).ThenInclude(si => si.Product)
            .Include(o => o.ServiceItems)
            .Include(o => o.Payments).ThenInclude(p => p!.Customer)
            .Include(o => o.Photos)
            .Include(o => o.CreatedUser)
            .Include(o => o.LastUpdatedUser)
            .Include(o => o.DeletedUser)
            .SingleOrDefaultAsync(o => o.Id == id)
            ?? throw new NotFoundException();

        OrderExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetOrderExistingAuthorization(order);

        return new(order, authorization);
    }

    public async Task<int> CreateAsync(OrderUpsertRequestDto requestDto)
    {
        if (!_authorizationService.CanCreateOrder())
        {
            throw new AuthorizationException();
        }

        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet();
        });

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        Customer customer = await GetCustomerOrCreateAsync(requestDto);

        DateTime currentDateTime = _clock.Now;
        Order order = new()
        {
            Type = requestDto.Type,
            StatsDate = requestDto.StatsDate ?? DateOnly.FromDateTime(currentDateTime),
            Note = requestDto.Note,
            CustomerId = customer.Id,
            CreatedDateTime = currentDateTime,
            CreatedUserId = _callerDetailProvider.GetId(),
        };

        _context.Orders.Add(order);

        if (order.Type is OrderType.Retail or OrderType.Treatment)
        {
            List<OrderProductItem> productItems = await _hasProductService.CreateItemsAsync(
                requestDto.ProductItems,
                MapProductItem,
                product => product.StockingQuantity -= 1,
                nameof(requestDto.ProductItems));

            order.ProductItems.AddRange(productItems);
        }

        if (order.Type is OrderType.Consultant or OrderType.Treatment)
        {
            List<string> processedServiceNames = new();
            for (int index = 0; index < requestDto.ServiceItems.Count; index += 1)
            {
                OrderServiceItemUpsertRequestDto itemRequestDto = requestDto.ServiceItems[index];
                
                if (processedServiceNames.Contains(itemRequestDto.Name))
                {
                    throw OperationException.Duplicated(
                        new object[] { nameof(requestDto.ServiceItems), index, nameof(itemRequestDto.Name) },
                        DisplayNames.Name
                    );
                }

                OrderServiceItem serviceItem = new()
                {
                    Name = itemRequestDto.Name,
                    AmountBeforeVatPerUnit = itemRequestDto.AmountBeforeVatPerUnit,
                    VatPercentagePerUnit = itemRequestDto.VatPercentagePerUnit,
                    Quantity = itemRequestDto.Quantity
                };

                order.ServiceItems.Add(serviceItem);
                processedServiceNames.Add(serviceItem.Name);
            }
        }

        order.ComputeCachedProperties();

        // TODO: Handle photo creation.

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateHandledException(exception);
            throw;
        }

        if (requestDto.PaidAmount > 0)
        {
            await CreatePaymentAsync(requestDto, order);
        }

        long debtAmount = order.CachedAmountAfterVat - requestDto.PaidAmount;
        if (debtAmount > 0)
        {
            long NewDebtAmountComputer(long amount) => amount + debtAmount;
            try
            {
                await _customerService.UpdateCachedDebtAmount(customer, NewDebtAmountComputer);
            }
            catch (NotFoundException)
            {
                throw new ConcurrencyException();
            }
        }

        await transaction.CommitAsync();
        
        return order.Id;
    }

    public async Task UpdateAsync(int id, OrderUpsertRequestDto requestDto)
    {
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet();
        });

        Order order = await _context.Orders
            .Include(o => o.ProductItems).ThenInclude(o => o.Product)
            .Include(o => o.ServiceItems)
            .Include(o => o.Payments)
            .AsSplitQuery()
            .SingleOrDefaultAsync(o => o.Id == id && o.DeletedDateTime == null)
            ?? throw new NotFoundException();

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        Customer customer = await GetCustomerOrCreateAsync(requestDto);
        long oldDebtAmount = order.AmountAfterVat - (order.EffectivePayment?.Amount ?? 0);

        order.StatsDate = requestDto.StatsDate ?? order.StatsDate;
        order.Note = requestDto.Note;
        order.LastUpdatedDateTime = _clock.Now;
        order.LastUpdatedUserId = _callerDetailProvider.GetId();

        if (order.Type is OrderType.Retail or OrderType.Treatment)
        {
            await _hasProductService.UpdateItemsAsync(
                requestDto.ProductItems,
                order.ProductItems,
                MapProductItem,
                product => product.StockingQuantity -= 1,
                nameof(requestDto.ProductItems));
        }

        if (order.Type is OrderType.Consultant or OrderType.Treatment)
        {
            IEnumerable<int> requestedServiceItemIds = requestDto.ServiceItems.Select(osi => osi.Id).OfType<int>();
            IEnumerable<OrderServiceItem> serviceItemsToBeDeleted = order.ServiceItems
                .Where(osi => !requestedServiceItemIds.Contains(osi.Id));

            foreach (OrderServiceItem serviceItem in serviceItemsToBeDeleted)
            {
                order.ServiceItems.Remove(serviceItem);
            }

            List<string> processedServiceNames = new();
            for (int index = 0; index < requestDto.ServiceItems.Count; index += 1)
            {
                OrderServiceItemUpsertRequestDto itemRequestDto = requestDto.ServiceItems[index];
                OrderServiceItem serviceItem;
                
                if (processedServiceNames.Contains(itemRequestDto.Name))
                {
                    throw OperationException.Duplicated(
                        new object[] { nameof(requestDto.ServiceItems), index, nameof(itemRequestDto.Name) },
                        DisplayNames.Name
                    );
                }

                if (!itemRequestDto.Id.HasValue)
                {
                    serviceItem = new()
                    {
                        Name = itemRequestDto.Name,
                        AmountBeforeVatPerUnit = itemRequestDto.AmountBeforeVatPerUnit,
                        VatPercentagePerUnit = itemRequestDto.VatPercentagePerUnit,
                        Quantity = itemRequestDto.Quantity
                    };

                    order.ServiceItems.Add(serviceItem);
                }
                else
                {
                    serviceItem = order.ServiceItems
                        .SingleOrDefault(osi => osi.Id == itemRequestDto.Id)
                        ?? throw OperationException.NotFound(
                            new object[] { nameof(requestDto.ServiceItems), index, nameof(itemRequestDto.Id) },
                            DisplayNames.OrderServiceItem
                        );

                    serviceItem.Name = itemRequestDto.Name;
                    serviceItem.AmountBeforeVatPerUnit = itemRequestDto.AmountBeforeVatPerUnit;
                    serviceItem.VatPercentagePerUnit = itemRequestDto.VatPercentagePerUnit;
                }

                processedServiceNames.Add(serviceItem.Name);
            }
        }

        order.ComputeCachedProperties();

        // TODO: Handle photo creation and updation.

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateHandledException(exception);
            throw;
        }

        if (order.EffectivePayment is null && requestDto.PaidAmount > 0)
        {
            await CreatePaymentAsync(requestDto, order);
        }
        else if (order.EffectivePayment is not null)
        {
            await UpdateOrDeletePaymentAsync(order.EffectivePayment, requestDto);
        }

        long newDebtAmount = order.AmountAfterVat - (order.EffectivePayment?.Amount ?? 0);
        long ComputeNewCachedDebtAmount(long amount) => amount + (newDebtAmount - oldDebtAmount);
        await _customerService.UpdateCachedDebtAmount(customer, ComputeNewCachedDebtAmount);
    
        await transaction.CommitAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Order order = await _context.Orders
            .Include(o => o.ProductItems).ThenInclude(oi => oi.Product)
            .SingleOrDefaultAsync(o => o.Id == id && o.DeletedDateTime == null)
            ?? throw new NotFoundException();

        OrderExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetOrderExistingAuthorization(order);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        order.DeletedDateTime = _clock.Now;
        order.DeletedUserId = _callerDetailProvider.GetId();
        
        _hasProductService.DeleteItemsAsync(order.ProductItems, (product) => product.StockingQuantity += 1);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateHandledException(exception);
            throw;
        }
    }

    public async Task<CountOverTimeRangeResponseDto<long>> GetRevenueAsync(CountOverTimeRangeRequestDto requestDto)
    {
        return await _topAndCountService.GetCountAsync(requestDto, (minDate, maxDate) =>
        {
            return _context.Orders
                .Where(o => o.DeletedDateTime == null)
                .Where(o => o.StatsDate >= minDate && o.StatsDate <= maxDate)
                .SumAsync(o => o.CachedAmountAfterVat);
        });
    }

    public async Task<CountOverTimeRangeResponseDto<int>> GetCountAsync(CountOverTimeRangeRequestDto requestDto)
    {
        return await _topAndCountService.GetCountAsync(requestDto, (minDate, maxDate) =>
        {
            return _context.Orders
                .Where(o => o.DeletedDateTime == null)
                .Where(o => o.StatsDate >= minDate && o.StatsDate <= maxDate)
                .CountAsync();
        });
    }

    public async Task<CountOverTimeRangeResponseDto<int>> GetConsultantCountAsync(
        CountOverTimeRangeRequestDto requestDto)
    {
        return await GetOrderCountAsync(requestDto, OrderType.Consultant);
    }

    public async Task<CountOverTimeRangeResponseDto<int>> GetRetailCountAsync(CountOverTimeRangeRequestDto requestDto)
    {
        return await GetOrderCountAsync(requestDto, OrderType.Retail);
    }

    public async Task<CountOverTimeRangeResponseDto<int>> GetTreatmentCountAsync(
        CountOverTimeRangeRequestDto requestDto)
    {
        return await GetOrderCountAsync(requestDto, OrderType.Treatment);
    }

    public async Task<List<StatsMonthYearResponseDto>> GetStatsMonthYearSeriesAsync()
    {
        return await _statsMonthYearService.GetStatsMonthYearSeries(_context.Orders);
    }
    #endregion

    #region PrivateMethods
    private async Task<Customer> GetCustomerOrCreateAsync(OrderUpsertRequestDto requestDto)
    {
        try
        {
            return await _customerService.GetOrCreateAsync(requestDto.Customer.Id, requestDto.Customer.Create!);
        }
        catch (OperationException exception)
        {
            exception.AddPropertyPathElementToTheBeginning(new object[] { nameof(requestDto.Customer) });
            throw;
        }
    }

    private async Task<CountOverTimeRangeResponseDto<int>> GetOrderCountAsync(
        CountOverTimeRangeRequestDto requestDto,
        OrderType? type = null)
    {
        return await _topAndCountService.GetCountAsync(requestDto, (minDate, maxDate) =>
        {
            IQueryable<Order> query = _context.Orders
                .Where(o => o.DeletedDateTime == null)
                .Where(o => o.StatsDate >= minDate && o.StatsDate <= maxDate);

            if (type is not null)
            {
                query = query.Where(o => o.Type == type);
            }

            return query.CountAsync();
        });
    }

    private async Task CreatePaymentAsync(OrderUpsertRequestDto orderRequestDto, Order order)
    {
        PaymentCreateRequestDto paymentRequestDto = new()
        {
            StatsDate = order.StatsDate,
            Type = PaymentType.OrderPayment,
            Amount = orderRequestDto.PaidAmount,
            CustomerId = order.CustomerId,
            OrderId = order.Id
        };

        await _paymentService.CreateWithoutValidationAsync(paymentRequestDto);
    }

    private async Task UpdateOrDeletePaymentAsync(Payment payment, OrderUpsertRequestDto orderRequestDto)
    {
        if (orderRequestDto.PaidAmount is 0)
        {
            await _paymentService.DeleteAsync(payment.Id);
            return;
        }

        PaymentUpdateRequestDto paymentRequestDto = new()
        {
            StatsDate = orderRequestDto.StatsDate,
            Amount = orderRequestDto.PaidAmount,
        };

        await _paymentService.UpdateWithoutValidationAsync(payment, paymentRequestDto);
    }

    private void ThrowDbUpdateHandledException(DbUpdateException exception)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is null)
        {
            return;
        }

        if (handledResult.IsConcurrencyConflict || handledResult.IsUniqueConstraintViolation)
        {
            throw new ConcurrencyException();
        }

        if (handledResult.IsForeignKeyConstraintViolation)
        {
            if (handledResult.ViolatedPropertyName == nameof(Order.CustomerId))
            {
                object[] propertyPathElements = new object[]
                {
                    nameof(OrderUpsertRequestDto.Customer),
                    nameof(OrderUpsertRequestDto.Customer.Id)
                };
                
                throw OperationException.NotFound(propertyPathElements, DisplayNames.Customer);
            }

            throw new ConcurrencyException();
        }
    }
    #endregion

    #region StaticMethods
    private static void MapProductItem(OrderProductItemUpsertRequestDto itemRequestDto, OrderProductItem item)
    {
        item.AmountBeforeVatPerUnit = itemRequestDto.AmountBeforeVatPerUnit;
        item.VatPercentagePerUnit = itemRequestDto.VatPercentagePerUnit;
    }
    #endregion
}
