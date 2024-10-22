namespace NATSInternal.Services;

/// <inheritdoc cref="ISupplyService" />
internal class SupplyService
    : ProductEngageableAbstractService<
        Supply,
        SupplyItem,
        SupplyPhoto,
        SupplyUpdateHistory,
        SupplyListRequestDto,
        SupplyItemRequestDto,
        SupplyUpdateHistoryDataDto>,
    ISupplyService
{
    private readonly DatabaseContext _context;
    private readonly IMultiplePhotosService<Supply, SupplyPhoto> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService<Supply, SupplyUpdateHistory> _statsService;

    public SupplyService(
            DatabaseContext context,
            IMultiplePhotosService<Supply, SupplyPhoto> photoservice,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService<Supply, SupplyUpdateHistory> statsService)
        : base(context, authorizationService)
    {
        _context = context;
        _photoService = photoservice;
        _authorizationService = authorizationService;
        _statsService = statsService;
    }

    /// <inheritdoc />
    public async Task<SupplyListResponseDto> GetListAsync(SupplyListRequestDto requestDto)
    {
        // Initialize query.
        IQueryable<Supply> query = _context.Supplies
            .Include(s => s.CreatedUser).ThenInclude(u => u.Roles)
            .Include(s => s.Items)
            .Include(s => s.Photos);

        // Sorting directing and sorting by field.
        Expression<Func<Supply, long>> amountExpression = (Supply supply) =>
            supply.Items.Sum(s => s.ProductAmountPerUnit * s.Quantity) + supply.ShipmentFee;
        switch (requestDto.OrderByField)
        {
            case nameof(OrderByFieldOptions.Amount):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(amountExpression).ThenBy(s => s.StatsDateTime)
                    : query.OrderByDescending(amountExpression)
                        .ThenByDescending(s => s.StatsDateTime);
                break;
            case nameof(OrderByFieldOptions.StatsDateTime):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(s => s.StatsDateTime).ThenBy(amountExpression)
                    : query.OrderByDescending(s => s.StatsDateTime)
                        .ThenByDescending(amountExpression);
                break;
            default:
                throw new NotImplementedException();
        }

        // Fetch the list of the entities.
        EntityListDto<Supply> entityListDto = await GetListOfEntitiesAsync(query, requestDto);

        return new SupplyListResponseDto
        {
            PageCount = entityListDto.PageCount,
            Items = entityListDto.Items?
                .Select(supply => new SupplyBasicResponseDto(
                    supply,
                    _authorizationService.GetSupplyAuthorization(supply)))
                .ToList(),
            MonthYearOptions = await GenerateMonthYearOptions(),
            Authorization = _authorizationService.GetSupplyListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<SupplyDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<Supply> query = _context.Supplies
            .Include(s => s.Items).ThenInclude(si => si.Product)
            .Include(s => s.Photos)
            .Include(s => s.CreatedUser).ThenInclude(u => u.Roles);

        // Fetch the entity.
        Supply supply = await GetEntityAsync(query, id);

        // Get the authorization information.
        SupplyAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetSupplyAuthorization(supply);

        return new SupplyDetailResponseDto(supply, authorization);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(SupplyUpsertRequestDto requestDto)
    {
        // Use a transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine the SupplyDateTime.
        DateTime paidDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Check if the current user has permission to specify the SupplyDateTime.
            if (!_authorizationService.CanSetSupplyStatsDateTime())
            {
                throw new AuthorizationException();
            }

            paidDateTime = requestDto.StatsDateTime.Value;
        }

        // Initialize entity.
        Supply supply = new Supply
        {
            StatsDateTime = paidDateTime,
            ShipmentFee = requestDto.ShipmentFee,
            Note = requestDto.Note,
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            CreatedUserId = _authorizationService.GetUserId(),
            Items = new List<SupplyItem>(),
            Photos = new List<SupplyPhoto>()
        };
        _context.Supplies.Add(supply);

        // Initialize items
        await CreateItemsAsync(supply.Items, requestDto.Items, ProductEngagementType.Import);

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

            HandleCreateOrUpdateException(sqlException, requestDto);
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
        DateOnly oldPaidDate = DateOnly.FromDateTime(supply.StatsDateTime);

        // Determining SupplyDateTime.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Restrict the SupplyDateTime to be modified after being locked.
            if (supply.IsLocked)
            {
                string errorMessage = ErrorMessages.CannotSetDateTimeAfterLocked
                    .ReplaceResourceName(DisplayNames.Supply)
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(
                    nameof(requestDto.StatsDateTime),
                    errorMessage);
            }

            // Validate SupplyDateTime.
            try
            {
                supply.StatsDateTime = requestDto.StatsDateTime.Value;
            }
            catch (ArgumentException exception)
            {
                string errorMessage = exception.Message
                    .ReplacePropertyName(DisplayNames.PaidDateTime);
                throw new OperationException(
                    nameof(requestDto.StatsDateTime),
                    errorMessage);
            }

            supply.StatsDateTime = requestDto.StatsDateTime.Value;
        }

        // Update supply properties.
        supply.ShipmentFee = requestDto.ShipmentFee;
        supply.Note = requestDto.Note;

        // Update supply items.
        await UpdateItemsAsync(supply.Items, requestDto.Items, ProductEngagementType.Import);

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
        LogUpdateHistory(supply, oldData, newData, requestDto.UpdatedReason);

        // Perform the updating operation.
        try
        {
            await _context.SaveChangesAsync();

            // The supply can be saved without any error, adjust the stats.
            // Revert the old stats.
            await _statsService.IncrementSupplyCostAsync(-oldItemAmount, oldPaidDate);
            await _statsService.IncrementShipmentCostAsync(-oldShipmentFee, oldPaidDate);

            // Add new stats.
            DateOnly newPaidDate = DateOnly.FromDateTime(supply.StatsDateTime);
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
                HandleCreateOrUpdateException(sqlException, requestDto);
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
        DeleteItems(
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
            DateOnly paidDate = DateOnly.FromDateTime(supply.StatsDateTime);
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

    /// <inheritdoc />
    protected override DbSet<Supply> GetRepository(DatabaseContext context)
    {
        return context.Supplies;
    }

    /// <inheritdoc />
    protected override DbSet<SupplyItem> GetItemRepository(DatabaseContext context)
    {
        return context.SupplyItems;
    }

    /// <inheritdoc />
    protected override bool CanAccessUpdateHistories(IAuthorizationInternalService service)
    {
        return service.CanAccessSupplyUpdateHistories();
    }

    /// <summary>
    /// Convert all the exceptions those are thrown by the database during the creating or
    /// updating operation into the appropriate execptions.
    /// </summary>
    /// <param name="exception">
    /// The exeception thrown by the database.
    /// </param>
    /// <param name="requestDto">
    /// The DTO containing data for the operation which caused the exception.
    /// </param>
    /// <exception cref="OperationException">
    /// Throws when the some item references to a the product which doesn't exist or has
    /// already been deleted.
    /// </exception>
    private static void HandleCreateOrUpdateException(
            MySqlException exception,
            SupplyUpsertRequestDto requestDto)
    {
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        if (exceptionHandler.IsForeignKeyNotFound &&
            exceptionHandler.ViolatedFieldName == nameof(SupplyItem.ProductId))
        {
            int productId = Convert.ToInt32(exceptionHandler.ViolatedValue);
            int index = requestDto.Items.FindIndex(i => i.ProductId == productId);
            string errorMessage = ErrorMessages.NotFound
                .ReplaceResourceName(DisplayNames.Product);
            throw new OperationException($"items[{index}]", errorMessage);
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
        SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(exception);
        if (exceptionHandler.IsDeleteOrUpdateRestricted)
        {
            string errorMessage = ErrorMessages.DeleteRestricted
                .ReplaceResourceName(DisplayNames.Supply);
            throw new OperationException(errorMessage);
        }
    }
}