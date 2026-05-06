using JetBrains.Annotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
// using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Orders;

[UsedImplicitly]
internal class OrderService : IOrderService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IHasProductService<OrderUpsertProductItemRequestDto, OrderProductItem> _hasProductService;
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
        IListFetchingService listFetchingService,
        IHasProductService<OrderUpsertProductItemRequestDto, OrderProductItem> hasProductService,
        IAuthorizationInternalService authorizationService,
        IValidator<OrderListRequestDto> listValidator,
        IValidator<OrderUpsertRequestDto> upsertValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _hasProductService = hasProductService;
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
            .Include(s => s.Photos.Where(photo => photo.IsThumbnail))
            .Where(s => s.DeletedDateTime == null);

        if (requestDto.StatsMonthYear is not null)
        {
            query = query.HasStatsMonthYear(requestDto.StatsMonthYear.Year, requestDto.StatsMonthYear.Month);
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(OrderListRequestDto.FieldToSort.StatsDateTime):
                query = query
                    .ApplySorting(s => s.StatsDate, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.CreatedDateTime):
                query = query
                    .ApplySorting(s => s.CreatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.LastUpdatedDateTime):
                query = query
                    .ApplySorting(s => s.LastUpdatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.ProductItemsAmount):
                query = query
                    .ApplySorting(
                        s => s.CachedProductItemsAmountBeforeVat + s.CachedProductItemsVatAmount,
                        requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.ServiceItemsAmount):
                query = query
                    .ApplySorting(
                        s => s.CachedServiceItemsAmountBeforeVat + s.CachedServiceItemsVatAmount,
                        requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(OrderListRequestDto.FieldToSort.TotalAmount):
                query = query
                    .ApplySorting(
                        s =>
                            s.CachedProductItemsAmountBeforeVat +
                            s.CachedProductItemsVatAmount +
                            s.CachedServiceItemsAmountBeforeVat +
                            s.CachedServiceItemsVatAmount,
                        requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDate, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Order> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<OrderBasicResponseDto> supplyResponseDtos = queryResult.Items
            .Select(o => new OrderBasicResponseDto(o, _authorizationService.GetOrderExistingAuthorization(o)))
            .ToList();

        return new(supplyResponseDtos, queryResult.ItemCount, queryResult.PageCount);
    }

    public async Task<OrderDetailResponseDto> GetDetailAsync(int id)
    {
        Order order = await _context.Orders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.ProductItems).ThenInclude(si => si.Product)
            .Include(s => s.ServiceItems)
            .Include(s => s.Photos)
            .Include(s => s.CreatedUser)
            .Include(s => s.LastUpdatedUser)
            .Include(s => s.DeletedUser)
            .SingleOrDefaultAsync(s => s.Id == id)
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

        DateTime currentDateTime = _clock.Now;
        Order order = new()
        {
            Type = requestDto.Type,
            StatsDate = requestDto.StatsDate ?? DateOnly.FromDateTime(currentDateTime),
            Note = requestDto.Note,
            CustomerId = requestDto.CustomerId,
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
                OrderUpsertServiceItemRequestDto itemRequestDto = requestDto.ServiceItems[index];
                
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
                    VatAmountPerUnit = itemRequestDto.VatAmountPerUnit,
                    Quantity = itemRequestDto.Quantity
                };

                order.ServiceItems.Add(serviceItem);
                processedServiceNames.Add(serviceItem.Name);
            }
        }

        // TODO: Handle photo creation.

        try
        {
            await _context.SaveChangesAsync();
            return order.Id;
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateHandledException(exception);
            throw;
        }
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
            .AsSplitQuery()
            .SingleOrDefaultAsync(o => o.Id == id && o.DeletedDateTime == null)
            ?? throw new NotFoundException();

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
                OrderUpsertServiceItemRequestDto itemRequestDto = requestDto.ServiceItems[index];
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
                        VatAmountPerUnit = itemRequestDto.VatAmountPerUnit,
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
                    serviceItem.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit;
                }

                order.ServiceItems.Add(serviceItem);
                processedServiceNames.Add(serviceItem.Name);
            }
        }

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
    #endregion

    #region PrivateMethods
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
                object[] propertyPathElements = new object[] { nameof(OrderUpsertRequestDto.CustomerId) };
                throw OperationException.NotFound(propertyPathElements, DisplayNames.Customer);
            }

            throw new ConcurrencyException();
        }
    }
    #endregion

    #region StaticMethods
    private static void MapProductItem(OrderUpsertProductItemRequestDto itemRequestDto, OrderProductItem item)
    {
        item.AmountBeforeVatPerUnit = itemRequestDto.AmountBeforeVatPerUnit;
        item.VatAmountPerUnit = itemRequestDto.VatAmountPerUnit;
    }
    #endregion
}
