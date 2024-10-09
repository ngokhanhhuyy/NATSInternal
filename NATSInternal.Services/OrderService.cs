namespace NATSInternal.Services;

/// <inheritdoc />
internal class OrderService : LockableEntityService, IOrderService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService<Order, OrderPhoto> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService<Order, OrderUpdateHistory> _statsService;
    private readonly IUpdateHistoryService<Order, OrderUpdateHistory, OrderUpdateHistoryDataDto> _updateHistoryService;
    private readonly IProductEngagementService<OrderItem, OrderPhoto, OrderUpdateHistory> _productEngagementService;
    private readonly IMonthYearService<Order, OrderUpdateHistory> _monthYearService;

    public OrderService(
        DatabaseContext context,
        IPhotoService<Order, OrderPhoto> photoService,
        IAuthorizationInternalService authorizationService,
        IStatsInternalService<Order, OrderUpdateHistory> statsService,
        IUpdateHistoryService<Order, OrderUpdateHistory, OrderUpdateHistoryDataDto> updateHistoryService,
        IProductEngagementService<OrderItem, OrderPhoto, OrderUpdateHistory> productEngagementService,
        IMonthYearService<Order, OrderUpdateHistory> monthYearService)
    {
        _context = context;
        _photoService = photoService;
        _authorizationService = authorizationService;
        _statsService = statsService;
        _updateHistoryService = updateHistoryService;
        _productEngagementService = productEngagementService;
        _monthYearService = monthYearService;
    }

    /// <inheritdoc />
    public async Task<OrderListResponseDto> GetListAsync(OrderListRequestDto requestDto)
    {
        // Initialize list of month and year options.
        List<MonthYearResponseDto> monthYearOptions = await _monthYearService
            .GenerateMonthYearOptions(dbContext => dbContext.Orders);

        // Initialize query.
        IQueryable<Order> query = _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.CreatedUser).ThenInclude(u => u.Roles)
            .Include(o => o.Items)
            .Include(o => o.Photos);

        // Sort by the specified direction and field.
        switch (requestDto.OrderByField)
        {
            case nameof(OrderListRequestDto.FieldOptions.Amount):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(o => o.Items.Sum(i => i.AmountPerUnit))
                        .ThenBy(o => o.PaidDateTime)
                    : query.OrderByDescending(o => o.Items.Sum(i => i.AmountPerUnit))
                        .ThenByDescending(o => o.PaidDateTime);
                break;
            default:
                query = requestDto.OrderByAscending
                    ? query.OrderBy(o => o.PaidDateTime)
                        .ThenBy(o => o.Items.Sum(i => i.AmountPerUnit))
                    : query.OrderByDescending(o => o.PaidDateTime)
                        .ThenByDescending(o => o.Items.Sum(i => i.AmountPerUnit));
                break;
        }

        // Filter by month and year if specified.
        if (!requestDto.IgnoreMonthYear)
        {
            DateTime startDateTime;
            startDateTime = new DateTime(requestDto.Year, requestDto.Month, 1);
            DateTime endDateTime = startDateTime.AddMonths(1);
            query = query
                .Where(o => o.PaidDateTime >= startDateTime && o.PaidDateTime < endDateTime);
        }

        // Filter by user id if specified.
        if (requestDto.CreatedUserId.HasValue)
        {
            query = query.Where(o => o.CreatedUserId == requestDto.CreatedUserId);
        }

        // Filter by customer id if specified.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(o => o.CustomerId == requestDto.CustomerId);
        }

        // Filter by product id if specified.
        if (requestDto.ProductId.HasValue)
        {
            query = query.Where(o => o.Items.Any(oi => oi.ProductId == requestDto.ProductId));
        }

        // Filter by not being soft deleted.
        query = query.Where(o => !o.IsDeleted);

        // Initialize response dto.
        OrderListResponseDto responseDto = new OrderListResponseDto
        {
            MonthYearOptions = monthYearOptions,
            Authorization = _authorizationService.GetOrderListAuthorization()
        };

        int resultCount = await query.CountAsync();
        if (resultCount == 0)
        {
            responseDto.PageCount = 0;
            return responseDto;
        }

        responseDto.PageCount = (int)Math.Ceiling(
            (double)resultCount / requestDto.ResultsPerPage);
        responseDto.Items = await query
            .Select(o => new OrderBasicResponseDto(
                o,
                _authorizationService.GetOrderAuthorization(o)))
            .Skip(requestDto.ResultsPerPage * (requestDto.Page - 1))
            .Take(requestDto.ResultsPerPage)
            .AsSplitQuery()
            .ToListAsync();

        return responseDto;
    }

    /// <inheritdoc />
    public async Task<OrderDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<Order> query = _context.Orders
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Include(o => o.Photos)
            .Include(o => o.Customer)
            .Include(o => o.CreatedUser).ThenInclude(u => u.Roles);

        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = _authorizationService
            .CanAccessOrderUpdateHistories();
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(o => o.UpdateHistories);
        }

        // Fetch the entity with the given id from the database.
        Order order = await query
            .AsSplitQuery()
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(User),
                nameof(id),
                id.ToString());

        return new OrderDetailResponseDto(
            order,
            _authorizationService.GetOrderAuthorization(order),
            mapUpdateHistories: shouldIncludeUpdateHistories);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(OrderUpsertRequestDto requestDto)
    {
        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine ordered datetime.
        DateTime orderedDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.PaidDateTime.HasValue)
        {
            // Check if the current user has permission to specify the ordered datetime.
            if (!_authorizationService.CanSetOrderPaidDateTime())
            {
                throw new AuthorizationException();
            }

            // The ordered datetime is valid, assign it to the new order.
            orderedDateTime = requestDto.PaidDateTime.Value;
        }

        // Initialize order entity.
        Order order = new Order
        {
            PaidDateTime = orderedDateTime,
            Note = requestDto.Note,
            CustomerId = requestDto.CustomerId,
            CreatedUserId = _authorizationService.GetUserId(),
            Items = new List<OrderItem>(),
            Photos = new List<OrderPhoto>()
        };
        _context.Orders.Add(order);

        // Initialize order items entities.
        await _productEngagementService.CreateItemsAsync(
            order.Items,
            requestDto.Items,
            ProductEngagementType.Export);

        // Initialize photos.
        if (requestDto.Photos != null)
        {
            await _photoService.CreateMultipleAsync(order, requestDto.Photos);
        }

        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();

            // The order can be created successfully without any error. Add the order
            // to the stats.
            DateOnly orderedDate = DateOnly.FromDateTime(order.PaidDateTime);
            await _statsService.IncrementRetailGrossRevenueAsync(
                order.ProductAmountBeforeVat,
                orderedDate);
            if (order.ProductVatAmount > 0)
            {
                await _statsService.IncrementVatCollectedAmountAsync(
                    order.ProductVatAmount,
                    orderedDate);
            }

            // Commit the transaction, finish the operation.
            await transaction.CommitAsync();
            return order.Id;
        }
        catch (DbUpdateException exception)
        {
            // Remove all the created photos.
            foreach (OrderPhoto photo in order.Photos)
            {
                _photoService.Delete(photo.Url);
            }

            // Handle the concurency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle the operation exception and convert to the appropriate error.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                if (exceptionHandler.IsForeignKeyNotFound)
                {
                    string errorMessage;
                    switch (exceptionHandler.ViolatedFieldName)
                    {
                        case "customer_id":
                            errorMessage = ErrorMessages.NotFound
                                .ReplaceResourceName(DisplayNames.Customer);
                            break;
                        case "user_id":
                            errorMessage = ErrorMessages.NotFound
                                .ReplaceResourceName(DisplayNames.User);
                            break;
                        default:
                            errorMessage = ErrorMessages.Undefined;
                            break;
                    }
                    throw new OperationException(errorMessage);
                }
            }
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, OrderUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Order order = await _context.Orders
            .Include(o => o.CreatedUser)
            .Include(o => o.Customer)
            .Include(o => o.Items).ThenInclude(oi => oi.Product)
            .Include(o => o.Photos)
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Order),
                nameof(id),
                id.ToString());

        // Check if the current user has permission to edit this order.
        if (!_authorizationService.CanEditOrder(order))
        {
            throw new AuthorizationException();
        }

        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Storing the old data for update history logging and stats adjustments.
        long oldItemAmount = order.ProductAmountBeforeVat;
        long oldVatAmount = order.ProductVatAmount;
        DateOnly oldPaidDate = DateOnly.FromDateTime(order.PaidDateTime);
        OrderUpdateHistoryDataDto oldData = new OrderUpdateHistoryDataDto(order);

        // Handle the new ordered datetime when the request specifies it.
        if (requestDto.PaidDateTime.HasValue)
        {
            // Check if the current user has permission to specify a new ordered datetime.
            if (!_authorizationService.CanSetOrderPaidDateTime())
            {
                throw new AuthorizationException();
            }

            // Prevent the order's SupplyDateTime to be modified when the order is locked.
            if (order.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.Order)
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(
                    nameof(requestDto.PaidDateTime),
                    errorMessage);
            }

            // Assign the new SupplyDateTime value only if it's different from the old one.
            if (requestDto.PaidDateTime.Value != order.PaidDateTime)
            {
                // Validate the specified SupplyDateTime value from the request.
                try
                {
                    _statsService.ValidateStatsDateTime(order, requestDto.PaidDateTime.Value);
                    order.PaidDateTime = requestDto.PaidDateTime.Value;
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.PaidDateTime);
                    throw new OperationException(
                        nameof(requestDto.PaidDateTime),
                        errorMessage);
                }
            }
        }

        // Update order properties.
        order.Note = requestDto.Note;

        // Update order items.
        await _productEngagementService.UpdateItemsAsync(
            order.Items,
            requestDto.Items,
            ProductEngagementType.Export,
            DisplayNames.OrderItem);

        // Update photos.
        List<string> urlsToBeDeletedWhenSucceeds = new List<string>();
        List<string> urlsToBeDeletedWhenFails = new List<string>();
        if (requestDto.Photos != null)
        {
            (List<string>, List<string>) photoUpdateResults = await _photoService
                .UpdateMultipleAsync(order, requestDto.Photos);
            urlsToBeDeletedWhenSucceeds.AddRange(photoUpdateResults.Item1);
            urlsToBeDeletedWhenFails.AddRange(photoUpdateResults.Item2);
        }

        // Store new data for update history logging.
        OrderUpdateHistoryDataDto newData = new OrderUpdateHistoryDataDto(order);

        // Log update history.
        _updateHistoryService
            .LogUpdateHistory(order, oldData, newData, requestDto.UpdateReason);

        // Save changes and handle errors.
        try
        {
            // Save all modifications.
            await _context.SaveChangesAsync();

            // The order can be updated successfully without any error.
            // Adjust the stats for items' amount and vat collected amount.
            // Revert the old stats.
            await _statsService.IncrementRetailGrossRevenueAsync(-oldItemAmount, oldPaidDate);
            await _statsService.IncrementVatCollectedAmountAsync(-oldVatAmount, oldPaidDate);

            // Delete all old photos which have been replaced by new ones.
            DateOnly newPaidDate = DateOnly.FromDateTime(order.PaidDateTime);
            await _statsService
                .IncrementRetailGrossRevenueAsync(order.ProductAmountBeforeVat, newPaidDate);
            await _statsService.IncrementVatCollectedAmountAsync(order.ProductVatAmount, newPaidDate);

            // Delete photo files which have been specified.
            foreach (string url in urlsToBeDeletedWhenSucceeds)
            {
                _photoService.Delete(url);
            }

            // Commit the trasaction and finish the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Undo all the created photos.
            foreach (string url in urlsToBeDeletedWhenFails)
            {
                _photoService.Delete(url);
            }

            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handling the exception in the foreseen cases.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                if (exceptionHandler.IsForeignKeyNotFound)
                {
                    throw new ResourceNotFoundException(
                        nameof(Customer),
                        nameof(requestDto.CustomerId),
                        requestDto.CustomerId.ToString());
                }
            }
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity from the database and ensure it exists.
        Order order = await _context.Orders
            .SingleOrDefaultAsync(o => o.Id == id && !o.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Order),
                nameof(id),
                id.ToString());

        // Check if the current user has permission to delete the order.
        if (!_authorizationService.CanDeleteOrder(order))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        _productEngagementService.DeleteItems(
            order.Items,
            dbContext => dbContext.OrderItems,
            ProductEngagementType.Export);

        // Perform the deleting operation.
        try
        {
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle operation exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
                exceptionHandler.Handle(sqlException);
                if (exceptionHandler.IsDeleteOrUpdateRestricted)
                {
                    // Soft delete when there are any other related entities which are
                    // restricted to be deleted.
                    order.IsDeleted = true;

                    // Save changes.
                    await _context.SaveChangesAsync();

                    // Order has been deleted successfully, adjust the stats.
                    DateOnly orderedDate = DateOnly.FromDateTime(order.PaidDateTime);
                    await _statsService.IncrementRetailGrossRevenueAsync(
                        order.ProductAmountBeforeVat,
                        orderedDate);
                    await _statsService.IncrementVatCollectedAmountAsync(
                        order.ProductVatAmount,
                        orderedDate);

                    // Commit the transaction and finishing the operations.
                    await transaction.CommitAsync();
                }
            }

            throw;
        }
    }
}