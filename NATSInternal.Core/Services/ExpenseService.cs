namespace NATSInternal.Core.Services;

/// <inheritdoc cref="IExpenseService" />
internal class ExpenseService
    :
        HasStatsAbstractService<
            Expense,
            ExpenseUpdateHistory,
            ExpenseListRequestDto,
            ExpenseUpdateHistoryDataDto,
            ExpenseCreatingAuthorizationResponseDto,
            ExpenseExistingAuthorizationResponseDto>,
        IExpenseService
{
    private readonly DatabaseContext _context;
    private readonly IMultiplePhotosService<Expense, ExpensePhoto> _photoService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly ISummaryInternalService _statsService;

    public ExpenseService(
            DatabaseContext context,
            IMultiplePhotosService<Expense, ExpensePhoto> photoService,
            IAuthorizationInternalService authorizationService,
            ISummaryInternalService statsService)
        : base(context, authorizationService)
    {
        _context = context;
        _photoService = photoService;
        _authorizationService = authorizationService;
        _statsService = statsService;
    }

    /// <inheritdoc/>
    public async Task<ExpenseListResponseDto> GetListAsync(ExpenseListRequestDto requestDto)
    {
        // Initializing the query.
        IQueryable<Expense> query = _context.Expenses
            .Include(e => e.Photos);


        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortByFieldName
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.Amount):
                query = sortingByAscending
                    ? query.OrderBy(e => e.Amount)
                        .ThenBy(e => e.StatsDateTime)
                    : query.OrderByDescending(e => e.Amount)
                        .ThenByDescending(e => e.StatsDateTime);
                break;
            case nameof(OrderByFieldOption.StatsDateTime):
                query = sortingByAscending
                    ? query.OrderBy(e => e.StatsDateTime)
                        .ThenBy(e => e.Amount)
                    : query.OrderByDescending(e => e.StatsDateTime)
                        .ThenByDescending(e => e.Amount);
                break;
            default:
                throw new NotImplementedException();
        }

        // Fetch the entities.
        EntityListDto<Expense> listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new ExpenseListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items
                .Select(expense =>
                {
                    ExpenseExistingAuthorizationResponseDto authorization;
                    authorization = GetExistingAuthorization(expense);

                    return new ExpenseBasicResponseDto(expense, authorization);
                }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<ExpenseDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<Expense> query = _context.Expenses
            .Include(e => e.CreatedUser).ThenInclude(u => u.Roles)
            .Include(e => e.Payee)
            .Include(e => e.Photos);

        // Determine if the update histories should be fetched.
        if (CanAccessUpdateHistories())
        {
            query = query.Include(e => e.UpdateHistories);
        }

        // Fetch the entity with the given id and ensure it exists in the database.
        Expense expense = await query
            .AsSingleQuery()
            .SingleOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException(
                nameof(Expense),
                nameof(id),
                id.ToString());

        return new ExpenseDetailResponseDto(expense, GetExistingAuthorization(expense));
    }

    /// <inheritdoc/>
    public async Task<int> CreateAsync(ExpenseUpsertRequestDto requestDto)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine paid datetime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenCreating())
            {
                throw new AuthorizationException();
            }

            statsDateTime = requestDto.StatsDateTime.Value;
        }

        // Initialize expense.
        Expense expense = new Expense
        {
            Amount = requestDto.Amount,
            StatsDateTime = statsDateTime,
            Category = requestDto.Category,
            Note = requestDto.Note,
            CreatedUserId = _authorizationService.GetUserId(),
            Photos = new List<ExpensePhoto>()
        };

        _context.Expenses.Add(expense);

        // Set expense payee
        ExpensePayee payee = await _context.ExpensePayees
            .Where(ep => ep.Name == requestDto.PayeeName)
            .SingleOrDefaultAsync();

        if (payee == null)
        {
            payee = new ExpensePayee
            {
                Name = requestDto.PayeeName
            };
            expense.Payee = payee;
        }

        // Create expense photos.
        if (requestDto.Photos != null)
        {
            await _photoService.CreateMultipleAsync(expense, requestDto.Photos);
        }

        // Perform the creating operation.
        try
        {
            await _context.SaveChangesAsync();

            // Expense can be created successfully, adjust the stats.
            DateOnly paidDate = DateOnly.FromDateTime(statsDateTime);
            await _statsService
                .IncrementExpenseAsync(expense.Amount, expense.Category, paidDate);

            // Commit the transaction, finishing the operation.
            await transaction.CommitAsync();
            return expense.Id;
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            // Remove all created photos.
            foreach (string url in expense.Photos.Select(p => p.Url))
            {
                _photoService.Delete(url);
            }

            // Handle exception and convert to the appropriate exception.
            HandleException(sqlException);
            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException();
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, ExpenseUpsertRequestDto requestDto)
    {
        // Fetch the expense from the database and ensure it exists.
        Expense expense = await _context.Expenses
            .Include(e => e.CreatedUser)
            .Include(e => e.Payee)
            .Include(e => e.Photos)
            .AsSplitQuery()
            .SingleOrDefaultAsync(e => e.Id == id && !e.IsDeleted)
            ?? throw new NotFoundException(nameof(Expense), nameof(id), id.ToString());

        // Ensure the expense is editable by the requester.
        if (!CanEdit(expense))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Decrement the old stats.
        await _statsService.IncrementExpenseAsync(
            - expense.Amount,
            expense.Category,
            DateOnly.FromDateTime(expense.StatsDateTime));

        // Store the current data as the old data for update history logging.
        ExpenseUpdateHistoryDataDto oldData = new ExpenseUpdateHistoryDataDto(expense);

        // Determine the SupplyDateTime if the request has specified a value.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenEditing(expense))
            {
                throw new AuthorizationException();
            }

            // Assign the new SupplyDateTime value only if it's different from the old one.
            if (requestDto.StatsDateTime.Value != expense.StatsDateTime)
            {
                // Validate and assign the specified SupplyDateTime value from the request.
                try
                {
                    ValidateStatsDateTime(expense, requestDto.StatsDateTime.Value);
                    expense.StatsDateTime = requestDto.StatsDateTime.Value;
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.StatsDateTime);
                    throw new OperationException(
                        nameof(requestDto.StatsDateTime),
                        errorMessage);
                }
            }
        }

        // Update other fields.
        expense.Amount = requestDto.Amount;
        expense.Category = requestDto.Category;
        expense.Note = requestDto.Note;

        // Update payee.
        if (expense.Payee.Name != requestDto.PayeeName)
        {
            ExpensePayee payee = await _context.ExpensePayees
                .SingleOrDefaultAsync(ep => ep.Name == requestDto.PayeeName)
                ?? new ExpensePayee { Name = requestDto.PayeeName };

            // Remove old payee if there is no other expeses consuming it.
            bool isCurrentPayeeConsumed = await _context.Expenses
                .AnyAsync(e => e.Id != expense.Id && e.Payee.Id == expense.Payee.Id);
            if (!isCurrentPayeeConsumed)
            {
                _context.ExpensePayees.Remove(expense.Payee);
            }

            // Set new payee.
            expense.Payee = payee;
        }

        // Update photos.
        List<string> urlsToBeDeletedWhenSucceeded = new List<string>();
        List<string> urlsToBeDeletedWhenFailed = new List<string>();
        if (requestDto.Photos != null)
        {
            (List<string>, List<string>) photosUpdateResult = await _photoService
                .UpdateMultipleAsync(expense, requestDto.Photos);
            urlsToBeDeletedWhenSucceeded.AddRange(photosUpdateResult.Item1);
            urlsToBeDeletedWhenFailed.AddRange(photosUpdateResult.Item2);
        }

        // Storing the new data for update history logging.
        ExpenseUpdateHistoryDataDto newData = new ExpenseUpdateHistoryDataDto(expense);

        // Log update history.
        LogUpdateHistory(expense, oldData, newData, requestDto.UpdatedReason);

        // Perform the updating operation.
        try
        {
            await _context.SaveChangesAsync();

            // Add the new stats.
            await _statsService.IncrementExpenseAsync(
                expense.Amount,
                expense.Category,
                DateOnly.FromDateTime(expense.StatsDateTime));

            // Commit the transaction, finish the opeartion.
            await transaction.CommitAsync();

            // Clean the old photos.
            foreach (string url in urlsToBeDeletedWhenSucceeded)
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        {
            // Remove all created photos.
            foreach (string url in urlsToBeDeletedWhenFailed)
            {
                _photoService.Delete(url);
            }

            // Handle concurrency exception.
            if (exception is DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException();
            }

            // Handle operation exception.
            if (exception.InnerException is MySqlException sqlException)
            {
                HandleException(sqlException);
            }

            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        // Fetch the expense from the database and ensure it exists.
        Expense expense = await _context.Expenses
            .Include(e => e.Payee)
            .Include(e => e.Photos)
            .SingleOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException(nameof(Expense), nameof(id), id.ToString());

        // Ensure the user has permission to delete this expense.
        if (!CanDelete(expense))
        {
            throw new AuthorizationException();
        }

        // Remove expense.
        _context.Expenses.Remove(expense);

        // Remove expense payee.
        bool isCurrentPayeeConsumed = await _context.Expenses
            .Where(e => e.Id != expense.Id && e.Payee.Name == expense.Payee.Name)
            .AnyAsync();
        if (!isCurrentPayeeConsumed)
        {
            _context.ExpensePayees.Remove(expense.Payee);
        }

        // Remove expense photos.
        List<string> photoUrlsToBeDeletedWhenSucceeded = new List<string>();
        foreach (ExpensePhoto photo in expense.Photos)
        {
            photoUrlsToBeDeletedWhenSucceeded.Add(photo.Url);
            _context.ExpensePhotos.Remove(photo);
        }

        // Save changes.
        try
        {
            await _context.SaveChangesAsync();

            // The expense can be deleted sucessfully without any error, adjust the stats.
            await _statsService.IncrementExpenseAsync(
                -expense.Amount,
                expense.Category,
                DateOnly.FromDateTime(expense.StatsDateTime));

            // Remove all expense photos.
            foreach (string url in photoUrlsToBeDeletedWhenSucceeded)
            {
                _photoService.Delete(url);
            }
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlExecption)
        {
            HandleException(sqlExecption);

            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException();
        }
    }

    /// <inheritdoc cref="IExpenseService" />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Amount),
                DisplayName = DisplayNames.Amount
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.StatsDateTime),
                DisplayName = DisplayNames.StatsDateTime
            }
        };

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Single(i => i.Name == nameof(OrderByFieldOption.StatsDateTime))
                .Name,
            DefaultAscending = false
        };
    }

    /// <inheritdoc/>
    protected override DbSet<Expense> GetRepository(DatabaseContext context)
    {
        return context.Expenses;
    }

    /// <summary>
    /// Handles the exception thrown by the database when saving during the creating, updating
    /// or deleting operations.
    /// </summary>
    /// <param name="exception">
    /// The exception thrown by the database during the operation.
    /// </param>
    /// <exception cref="ConcurrencyException">
    /// Throws under the following circumstances:<br/>
    /// - When the <see cref="ExpensePayee"/> specified by the <c>PayeeName</c> in the request
    /// DTO exists when checking but not found when performing the saving action.<br/>
    /// - When the information of the requesting user has already been deleted before the
    /// operation.
    /// </exception>
    private static void HandleException(MySqlException exception)
    {
        SqlExceptionHandler handler = new SqlExceptionHandler(exception);

        // Handle foreign key not found cases in updating operation.
        if (handler.IsForeignKeyNotFound &&
            (handler.ViolatedFieldName == nameof(Expense.CreatedUserId) ||
            handler.ViolatedFieldName == nameof(Expense.PayeeId)))
        {
            switch (handler.ViolatedFieldName)
            {
                case nameof(Expense.CreatedUserId) or nameof(Expense.PayeeId):
                    throw new ConcurrencyException();
            }
        }

        // Handle delete restriction in deleting operation.
        else if (handler.IsDeleteOrUpdateRestricted)
        {
            throw new OperationException(ErrorMessages.DeleteRestricted
                .ReplaceResourceName(DisplayNames.Expense));
        }
    }
}