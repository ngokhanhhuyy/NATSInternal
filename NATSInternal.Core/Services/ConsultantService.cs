namespace NATSInternal.Core;

/// <inheritdoc cref="IConsultantService" />
internal class ConsultantService
    :
        HasStatsAbstractService<
            Consultant,
            ConsultantUpdateHistory,
            ConsultantListRequestDto,
            ConsultantUpdateHistoryDataDto,
            ConsultantCreatingAuthorizationResponseDto,
            ConsultantExistingAuthorizationResponseDto>,
        IConsultantService
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService _statsService;

    public ConsultantService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService statsService)
        : base(context, authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
        _statsService = statsService;
    }

    /// <inheritdoc />
    public async Task<ConsultantListResponseDto> GetListAsync(
            ConsultantListRequestDto requestDto)
    {
        // Initialize query.
        IQueryable<Consultant> query = _context.Consultants
            .Include(c => c.Customer);

        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByField
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        Expression<Func<Consultant, long>> amountExpression = (consultant) =>
            consultant.AmountBeforeVat + consultant.VatAmount;
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.Amount):
                query = sortingByAscending
                    ? query.OrderBy(amountExpression).ThenBy(e => e.StatsDateTime)
                    : query.OrderByDescending(amountExpression)
                        .ThenByDescending(e => e.StatsDateTime);
                break;
            case nameof(OrderByFieldOption.StatsDateTime):
                query = sortingByAscending
                    ? query.OrderBy(e => e.StatsDateTime).ThenBy(amountExpression)
                    : query.OrderByDescending(e => e.StatsDateTime)
                        .ThenByDescending(amountExpression);
                break;
            default:
                throw new NotImplementedException();
        }

        // Filter by customer id if specified.
        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(e => e.CustomerId == requestDto.CustomerId);
        }

        EntityListDto<Consultant> listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new ConsultantListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items?
                .Select(consultant => new ConsultantBasicResponseDto(
                    consultant,
                    GetExistingAuthorization(consultant)))
                .ToList(),
        };
    }

    /// <inheritdoc />
    public async Task<ConsultantDetailResponseDto> GetDetailAsync(int id)
    {
        // Initialize query.
        IQueryable<Consultant> query = _context.Consultants
            .Include(c => c.CreatedUser).ThenInclude(c => c.Roles)
            .Include(c => c.Customer);

        // Fetch the entity.
        Consultant consultant = await GetEntityAsync(query, id);

        return new ConsultantDetailResponseDto(
            consultant,
            GetExistingAuthorization(consultant));
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(ConsultantUpsertRequestDto requestDto)
    {
        // Use transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Determine StatsDateTime.
        DateTime statsDateTime = DateTime.UtcNow.ToApplicationTime();
        if (requestDto.StatsDateTime.HasValue)
        {
            // Check if the current user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenCreating())
            {
                throw new AuthorizationException();
            }

            statsDateTime = requestDto.StatsDateTime.Value;
        }

        // Initialize consultant.
        Consultant consultant = new Consultant
        {
            AmountBeforeVat = requestDto.AmountBeforeVat,
            VatAmount = requestDto.VatAmount,
            StatsDateTime = statsDateTime,
            Note = requestDto.Note,
            CustomerId = requestDto.CustomerId,
            CreatedUserId = _authorizationService.GetUserId()
        };
        _context.Consultants.Add(consultant);

        // Save changes and commit.
        try
        {
            await _context.SaveChangesAsync();

            // Consultant can be created successfully, adjust the stats.
            DateOnly paidDate = DateOnly.FromDateTime(statsDateTime);
            await _statsService
                .IncrementConsultantGrossRevenueAsync(consultant.AmountBeforeVat, paidDate);

            // Commit the transaction and finish the operation.
            await transaction.CommitAsync();
            return consultant.Id;
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            // Handle exception and convert to the appropriate exception.
            SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
            HandleCreateOrUpdateException(exceptionHandler);
            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException();
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, ConsultantUpsertRequestDto requestDto)
    {
        // Fetch the consultant from the database and ensure it exists.
        Consultant consultant = await _context.Consultants
            .Include(c => c.CreatedUser)
            .Include(c => c.Customer)
            .Where(c => c.Id == id && !c.IsDeleted)
            .AsSplitQuery()
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Expense),
                nameof(id),
                id.ToString());

        // Ensure the consultant is editable by the requester.
        if (!CanEdit(consultant))
        {
            throw new AuthorizationException();
        }

        // Using transaction for atomic operations.
        await using IDbContextTransaction transaction = await _context.Database
            .BeginTransactionAsync();

        // Store the old data for history logging and stats adjustment.
        ConsultantUpdateHistoryDataDto oldData;
        oldData = new ConsultantUpdateHistoryDataDto(consultant);
        await _statsService.IncrementConsultantGrossRevenueAsync(
            -consultant.AmountBeforeVat,
            DateOnly.FromDateTime(consultant.StatsDateTime));

        // Determining the StatsDateTime value based on the specified data from the request.
        if (requestDto.StatsDateTime.HasValue)
        {
            // Ensure the requesting user has permission to specify a value for StatsDateTime.
            if (!CanSetStatsDateTimeWhenEditing(consultant))
            {
                throw new AuthorizationException();
            }

            // Assign the new StatsDateTime value only if it's different from the old one.
            if (requestDto.StatsDateTime.Value != consultant.StatsDateTime)
            {
                // Validate the specfied StatsDateTime from the request.
                try
                {
                    ValidateStatsDateTime(consultant, requestDto.StatsDateTime.Value);
                }
                catch (ValidationException exception)
                {
                    string errorMessage = exception.Message
                        .ReplacePropertyName(DisplayNames.StatsDateTime);
                    throw new OperationException(
                        nameof(requestDto.StatsDateTime),
                        errorMessage);
                }

                consultant.StatsDateTime = requestDto.StatsDateTime.Value;
            }
        }

        // Update fields.
        consultant.AmountBeforeVat = requestDto.AmountBeforeVat;
        consultant.VatAmount = requestDto.VatAmount;
        consultant.Note = requestDto.Note;

        // Storing new data for update history logging.
        ConsultantUpdateHistoryDataDto newData;
        newData = new ConsultantUpdateHistoryDataDto(consultant);

        // Initialize update history.
        LogUpdateHistory(consultant, oldData, newData, requestDto.UpdatedReason);

        // Perform the updating operation.
        try
        {
            await _context.SaveChangesAsync();

            // Incurement new stats.
            await _statsService.IncrementConsultantGrossRevenueAsync(
                consultant.AmountBeforeVat,
                DateOnly.FromDateTime(consultant.StatsDateTime));

            // Commit the transaction and finishing the operation.
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlException)
        {
            // Handle exception and convert to the appropriate exception.
            SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
            HandleCreateOrUpdateException(exceptionHandler);
            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id)
    {
        // Fetch the consultant from the database and ensure it exists.
        Consultant consultant = await _context.Consultants
            .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted)
            ?? throw new ResourceNotFoundException(
                nameof(Consultant),
                nameof(id),
                id.ToString());

        // Ensure the consultant is editable by the requester.
        if (!CanEdit(consultant))
        {
            throw new AuthorizationException();
        }

        // Remove expense.
        _context.Consultants.Remove(consultant);

        // Perform the deleting operation.
        try
        {
            await _context.SaveChangesAsync();

            // The expense can be deleted sucessfully without any error, revert the stats.
            await _statsService.IncrementConsultantGrossRevenueAsync(
                -consultant.AmountBeforeVat,
                DateOnly.FromDateTime(consultant.StatsDateTime));
        }
        catch (DbUpdateException exception)
        when (exception.InnerException is MySqlException sqlExecption)
        {
            SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlExecption);
            if (exceptionHandler.IsDeleteOrUpdateRestricted)
            {
                string errorMessage = ErrorMessages.DeleteRestricted
                    .ReplaceResourceName(DisplayNames.Expense);
                throw new OperationException(errorMessage);
            }

            throw;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyException();
        }
    }

    /// <inheritdoc cref="IConsultantService.GetListSortingOptions"/>
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
    protected override DbSet<Consultant> GetRepository(DatabaseContext context)
    {
        return context.Consultants;
    }

    /// <summary>
    /// Handles the exception throws by the database when saving during the creating or
    /// updating operation.
    /// </summary>
    /// <param name="handler">
    /// An initialized exception handler that captured the exception.
    /// </param>
    /// <exception cref="ConcurrencyException">
    /// Throws when the information of the requesting user has already been deleted before
    /// the operation.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the referenced <see cref="Customer"/> who has id specfied by the value of
    /// the <c>CustomerId</c> property in the consultant doesn't exist or has already been
    /// deleted.
    /// </exception>
    private static void HandleCreateOrUpdateException(SqlExceptionHandler handler)
    {
        if (handler.IsForeignKeyNotFound)
        {
            switch (handler.ViolatedFieldName)
            {
                case nameof(Consultant.CreatedUserId):
                    throw new ConcurrencyException();
                case nameof(Consultant.CustomerId):
                    string errorMessage = ErrorMessages.NotFound
                        .ReplaceResourceName(DisplayNames.Customer);
                    throw new OperationException(nameof(Consultant.Customer), errorMessage);
            }
        }
    }
}