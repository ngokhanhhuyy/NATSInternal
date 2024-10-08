﻿namespace NATSInternal.Services;

/// <inheritdoc cref="ISupplyService" />
internal class SupplyService : LockableEntityService, ISupplyService
{
    private readonly DatabaseContext _context;
    private readonly IPhotoService<Supply, SupplyPhoto> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService<Supply, User, SupplyUpdateHistory> _statsService;
    private readonly IProductEngagementService<SupplyItem, Product, SupplyPhoto, User, SupplyUpdateHistory> _productEngagementService;
    private readonly IUpdateHistoryService<Supply, User, SupplyUpdateHistory, SupplyUpdateHistoryDataDto> _updateHistoryService;
    private readonly IMonthYearService<Supply, User, SupplyUpdateHistory> _monthYearService;

    public SupplyService(
            DatabaseContext context,
            IPhotoService<Supply, SupplyPhoto> photoservice,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<Supply, User, SupplyUpdateHistory> statsService,
            IProductEngagementService<SupplyItem, Product, SupplyPhoto, User, SupplyUpdateHistory> productEngagementService,
            IUpdateHistoryService<Supply, User, SupplyUpdateHistory, SupplyUpdateHistoryDataDto> updateHistoryService,
            IMonthYearService<Supply, User, SupplyUpdateHistory> monthYearService)
    {
        _context = context;
        _photoService = photoservice;
        _authorizationService = authorizationService;
        _statsService = statsService;
        _productEngagementService = productEngagementService;
        _updateHistoryService = updateHistoryService;
        _monthYearService = monthYearService;
    }

    /// <inheritdoc />
    public async Task<SupplyListResponseDto> GetListAsync(SupplyListRequestDto requestDto)
    {
        // Initialize list of month and year options.
        List<MonthYearResponseDto> monthYearOptions = await _monthYearService
            .GenerateMonthYearOptions(dbContext => dbContext.Supplies);

        // Query initialization.
        IQueryable<Supply> query = _context.Supplies
            .Include(s => s.CreatedUser).ThenInclude(u => u.Roles)
            .Include(s => s.Items)
            .Include(s => s.Photos);

        // Sorting directing and sorting by field.
        switch (requestDto.OrderByField)
        {
            case nameof(SupplyListRequestDto.FieldOptions.TotalAmount):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(s => s.TotalAmount)
                        .ThenBy(s => s.SuppliedDateTime)
                    : query.OrderByDescending(s => s.Items.Sum(i => i.AmountPerUnit))
                        .ThenByDescending(s => s.SuppliedDateTime);
                break;
            case nameof(SupplyListRequestDto.FieldOptions.PaidDateTime):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(s => s.SuppliedDateTime)
                        .ThenBy(s => s.Items.Sum(i => i.AmountPerUnit))
                    : query.OrderByDescending(s => s.SuppliedDateTime)
                        .ThenByDescending(s => s.Items.Sum(i => i.AmountPerUnit));
                break;
            case nameof(SupplyListRequestDto.FieldOptions.ItemAmount):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(s => s.ItemAmount)
                        .ThenBy(s => s.SuppliedDateTime)
                    : query.OrderByDescending(s => s.ItemAmount)
                        .ThenByDescending(s => s.SuppliedDateTime);
                break;
            case nameof(SupplyListRequestDto.FieldOptions.ShipmentFee):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(s => s.ShipmentFee)
                        .ThenBy(s => s.SuppliedDateTime)
                    : query.OrderByDescending(s => s.ShipmentFee)
                        .ThenByDescending(s => s.SuppliedDateTime);
                break;
        }

        // Filter by month and year if specified.
        if (!requestDto.IgnoreMonthYear)
        {
            DateTime startDateTime;
            startDateTime = new DateTime(requestDto.Year, requestDto.Month, 1);
            DateTime endDateTime = startDateTime.AddMonths(1);
            query = query
                .Where(s => s.SuppliedDateTime >= startDateTime && s.SuppliedDateTime < endDateTime);
        }

        // Filter by user id if specified.
        if (requestDto.CreatedUserId.HasValue)
        {
            query = query.Where(s => s.CreatedUserId == requestDto.CreatedUserId.Value);
        }

        // Filter by product id if specified.
        if (requestDto.ProductId.HasValue)
        {
            query = query.Where(s => s.Items.Any(si => si.ProductId == requestDto.ProductId));
        }

        // Initialize response dto.
        SupplyListResponseDto responseDto = new SupplyListResponseDto
        {
            MonthYearOptions = monthYearOptions,
            Authorization = _authorizationService.GetSupplyListAuthorization()
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
            .Select(s => new SupplyBasicResponseDto(s))
            .Skip(requestDto.ResultsPerPage * (requestDto.Page - 1))
            .Take(requestDto.ResultsPerPage)
            .AsSingleQuery()
            .ToListAsync();

        return responseDto;
    }

    /// <inheritdoc />
    public async Task<SupplyDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<Supply> query = _context.Supplies
            .Include(s => s.Items).ThenInclude(si => si.Product)
            .Include(s => s.Photos)
            .Include(s => s.CreatedUser).ThenInclude(u => u.Roles);

        // Determine if the update histories should be fetched.
        bool shouldIncludeUpdateHistories = _authorizationService
            .CanAccessSupplyUpdateHistories();
        if (shouldIncludeUpdateHistories)
        {
            query = query.Include(s => s.UpdateHistories);
        }

        // Fetch the entity with the given id and ensure it exists in the database.
        Supply supply = await query
            .AsSplitQuery()
            .SingleOrDefaultAsync(s => s.Id == id && !s.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Supply),
                nameof(id),
                id.ToString());

        return new SupplyDetailResponseDto(
            supply,
            _authorizationService.GetSupplyAuthorization(supply),
            mapUpdateHistories: shouldIncludeUpdateHistories);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(SupplyUpsertRequestDto requestDto)
    {
        // Use a transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine the SupplyDateTime.
        DateTime paidDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.PaidDateTime.HasValue)
        {
            // Check if the current user has permission to specify the SupplyDateTime.
            if (!_authorizationService.CanSetSupplyPaidDateTime())
            {
                throw new AuthorizationException();
            }

            paidDateTime = requestDto.PaidDateTime.Value;
        }

        // Initialize entity.
        Supply supply = new Supply
        {
            SuppliedDateTime = paidDateTime,
            ShipmentFee = requestDto.ShipmentFee,
            Note = requestDto.Note,
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            CreatedUserId = _authorizationService.GetUserId(),
            Items = new List<SupplyItem>(),
            Photos = new List<SupplyPhoto>()
        };
        _context.Supplies.Add(supply);

        // Initialize items
        await _productEngagementService
            .CreateItemsAsync(supply.Items, requestDto.Items, ProductEngagementType.Import);

        // Initialize photos
        if (requestDto.Photos != null)
        {
            await _photoService.CreateMultipleAsync(supply, requestDto.Photos);
        }

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();

            // The supply can be saved successfully without any error.
            // Add new stats for the created supply.
            await _statsService.IncrementSupplyCostAsync(supply.ItemAmount);
            await _statsService.IncrementShipmentCostAsync(supply.ShipmentFee);

            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();

            return supply.Id;
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            await transaction.RollbackAsync();
            // Delete all created photos.
            foreach (SupplyPhoto supplyPhoto in supply.Photos)
            {
                _photoService.Delete(supplyPhoto.Url);
            }

            HandleCreateOrUpdateException(sqlException);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, SupplyUpsertRequestDto requestDto)
    {
        // Fetch the entity from the database and ensure it exists.
        Supply supply = await _context.Supplies
            .Include(s => s.Items).ThenInclude(i => i.Product)
            .Include(s => s.Photos)
            .Where(s => s.Id == id)
            .AsSplitQuery()
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Supply),
                nameof(id),
                id.ToString());

        // Ensure the user has permission to edit the supply.
        if (!_authorizationService.CanEditSupply(supply))
        {
            throw new AuthorizationException();
        }

        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Storing the old data for update history logging and stats adjustments.
        SupplyUpdateHistoryDataDto oldData = new SupplyUpdateHistoryDataDto(supply);
        long oldItemAmount = supply.ItemAmount;
        long oldShipmentFee = supply.ShipmentFee;
        DateOnly oldPaidDate = DateOnly.FromDateTime(supply.SuppliedDateTime);

        // Determining SupplyDateTime.
        if (requestDto.PaidDateTime.HasValue)
        {
            // Restrict the SupplyDateTime to be modified after being locked.
            if (supply.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.Supply)
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(nameof(requestDto.PaidDateTime), errorMessage);
            }

            // Validate SupplyDateTime.
            try
            {
                supply.SuppliedDateTime = requestDto.PaidDateTime.Value;
            }
            catch (ArgumentException exception)
            {
                string errorMessage = exception.Message
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(nameof(requestDto.PaidDateTime), errorMessage);
            }

            supply.SuppliedDateTime = requestDto.PaidDateTime.Value;
        }

        // Update supply properties.
        supply.ShipmentFee = requestDto.ShipmentFee;
        supply.Note = requestDto.Note;

        // Update supply items.
        await _productEngagementService.UpdateItemsAsync(
            supply.Items,
            requestDto.Items,
            ProductEngagementType.Import,
            DisplayNames.SupplyItem);

        // Update photos.
        List<string> urlsToBeDeletedWhenSucceed = new List<string>();
        List<string> urlsToBeDeletedWhenFails = new List<string>();
        if (requestDto.Photos != null)
        {
            (List<string>, List<string>) photoUpdateResults = await _photoService
                .UpdateMultipleAsync(supply, requestDto.Photos);
            urlsToBeDeletedWhenSucceed.AddRange(photoUpdateResults.Item1);
            urlsToBeDeletedWhenFails.AddRange(photoUpdateResults.Item2);
        }

        // Storing new data for update history logging.
        SupplyUpdateHistoryDataDto newData = new SupplyUpdateHistoryDataDto(supply);

        // Log update history.
        _updateHistoryService
            .LogUpdateHistory(supply, oldData, newData, requestDto.UpdateReason);

        // Perform the updating operation.
        try
        {
            await _context.SaveChangesAsync();

            // The supply can be saved without any error, adjust the stats.
            // Revert the old stats.
            await _statsService.IncrementSupplyCostAsync(-oldItemAmount, oldPaidDate);
            await _statsService.IncrementShipmentCostAsync(-oldShipmentFee, oldPaidDate);

            // Add new stats.
            DateOnly newPaidDate = DateOnly.FromDateTime(supply.SuppliedDateTime);
            await _statsService.IncrementShipmentCostAsync(supply.ItemAmount, newPaidDate);
            await _statsService.IncrementShipmentCostAsync(supply.ShipmentFee, newPaidDate);

            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();
            foreach (string url in urlsToBeDeletedWhenSucceed)
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
            
            if (exception.InnerException is MySqlException sqlException)
            {
                await transaction.RollbackAsync();
                foreach (string url in urlsToBeDeletedWhenFails)
                {
                    _photoService.Delete(url);
                }
                HandleCreateOrUpdateException(sqlException);
            }
            
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        // Fetch the entity with given id from the database and ensure it exists.
        Supply supply = await _context.Supplies
            .Include(s => s.Items).ThenInclude(si => si.Product)
            .Include(s => s.Photos)
            .SingleOrDefaultAsync(s => s.Id == id)
            ?? throw new ResourceNotFoundException(
                nameof(Supply),
                nameof(id),
                id.ToString());

        if (!_authorizationService.CanDeleteSupply(supply))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Remove all the items.
        _productEngagementService.DeleteItems(
            supply.Items,
            dbContext => dbContext.SupplyItems,
            ProductEngagementType.Export);

        // Delete the supply entity.
        _context.Supplies.Remove(supply);

        try
        {
            await _context.SaveChangesAsync();

            // The supply can be deleted successfully without any error.
            // Revert the stats associated to the supply.
            DateOnly paidDate = DateOnly.FromDateTime(supply.SuppliedDateTime);
            await _statsService.IncrementSupplyCostAsync(-supply.ItemAmount, paidDate);
            await _statsService.IncrementSupplyCostAsync(-supply.ShipmentFee, paidDate);

            // Commit transaction and finish the operation.
            await transaction.CommitAsync();

            // Delete all supply photos after transaction succeeded.
            if (supply.Photos != null)
            {
                foreach (string url in supply.Photos.Select(p => p.Url))
                {
                    _photoService.Delete(url);
                }
            }
        }
        catch (DbUpdateException exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }
            
            if (exception.InnerException is MySqlException sqlException)
            {
               HandleDeleteExeption(sqlException);
            }
            
            throw;
        }

    }

    /// <summary>
    /// Convert all the exceptions those are thrown by the database during the creating
    /// or updating operation into the appropriate execptions.
    /// </summary>
    /// <param name="exception">The exeception thrown by the database.</param>
    /// <exception cref="OperationException"></exception>
    private static void HandleCreateOrUpdateException(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
        exceptionHandler.Handle(exception);
        if (exceptionHandler.IsForeignKeyNotFound)
        {
            if (exceptionHandler.ViolatedFieldName == "product_id")
            {
                string errorMessage = ErrorMessages.NotFound
                    .ReplaceResourceName(DisplayNames.Product);
                throw new OperationException($"items.productId", errorMessage);
            }
        }
    }

    /// <summary>
    /// Convert all the exceptions those are thrown by the database during
    /// the deleting operation into the appropriate exceptions.
    /// </summary>
    /// <param name="exception">The exception thrown by the database.</param>
    /// <exception cref="OperationException"></exception>
    private static void HandleDeleteExeption(MySqlException exception)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler();
        exceptionHandler.Handle(exception);
        if (exceptionHandler.IsDeleteOrUpdateRestricted)
        {
            string errorMessage = ErrorMessages.DeleteRestricted
                .ReplaceResourceName(DisplayNames.Supply);
            throw new OperationException(errorMessage);
        }
    }
}
