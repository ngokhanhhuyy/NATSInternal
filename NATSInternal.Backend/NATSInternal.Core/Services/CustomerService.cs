namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class CustomerService : ICustomerService
{
    #region Fields
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly IListQueryService _listQueryService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly ISummaryInternalService _summaryService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<CustomerListRequestDto> _listValidator;
    private readonly IValidator<CustomerUpsertRequestDto> _upsertValidator;
    #endregion

    #region Constructors
    public CustomerService(
            IDbContextFactory<DatabaseContext> contextFactory,
            IListQueryService listQueryService,
            IAuthorizationInternalService authorizationService,
            ISummaryInternalService summaryService,
            IDbExceptionHandler exceptionHandler,
            IValidator<CustomerListRequestDto> listValidator,
            IValidator<CustomerUpsertRequestDto> upsertValidator)
    {
        _contextFactory = contextFactory;
        _listQueryService = listQueryService;
        _authorizationService = authorizationService;
        _summaryService = summaryService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
    }
    #endregion

    #region Methods
    public async Task<CustomerListResponseDto> GetListAsync(
            CustomerListRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);

        // Generate a new instance of DbContext.
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        // Preparing the query.
        IQueryable<Customer> query = context.Customers
            .Include(c => c.CreatedUser).ThenInclude(u => u.Roles)
            .Include(c => c.Debts);

        // Add filter for search content.
        if (requestDto.SearchContent != null)
        {
            bool isValidBirthday = DateOnly.TryParse(requestDto.SearchContent, out DateOnly birthday);
            query = query.Where(c =>
                c.NormalizedFullName.Contains(requestDto.SearchContent.ToUpper()) ||
                c.PhoneNumber == null ? true : c.PhoneNumber.Contains(requestDto.SearchContent) ||
                (isValidBirthday && c.Birthday.HasValue && c.Birthday.Value == birthday));
        }

        // Filter by remaining debt amount.
        if (requestDto.HasRemainingDebtAmountOnly is true)
        {
            query = query.Where(c => c.CachedDebtAmount > 0);
        }

        // Determine the field and the direction the sort.
        string sortByFieldName = requestDto.SortByFieldName ?? GetListSortingOptions().DefaultFieldName;
        bool sortByAscending = requestDto.SortByAscending ?? GetListSortingOptions().DefaultAscending;
        switch (sortByFieldName)
        {
            case nameof(CustomerListRequestDto.FieldToSort.FirstName):
                query = query.ApplySorting(c => c.FirstName, sortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.Birthday):
                query = query.ApplySorting(c => c.Birthday, sortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(c => c.CreatedDateTime, sortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.DebtRemainingAmount):
                query = query.ApplySorting(c => c.CachedIncurredDebtAmount - c.CachedPaidDebtAmount, sortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.LastName):
                query = query
                    .ApplySorting(c => c.LastName, sortByAscending)
                    .ApplySorting(c => c.FirstName, sortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        return await _listQueryService.GetPagedListAsync(
            query,
            requestDto,
            (customers, pageCount) => new CustomerListResponseDto(customers, pageCount),
            cancellationToken
        );
    }

    /// <inheritdoc />
    public async Task<CustomerBasicResponseDto> GetBasicAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Generate a new instance of the DbContext.
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        // 
        Customer customer = await context.Customers
            .Include(c => c.Debts)
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        CustomerExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetExistingAuthorization<Customer, CustomerExistingAuthorizationResponseDto>();

        return new CustomerBasicResponseDto(customer, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<CustomerDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        Customer customer = await context.Customers
            .Include(customer => customer.Introducer)
            .Include(customer => customer.CreatedUser).ThenInclude(u => u.Roles)
            .Include(customer => customer.Debts.OrderByDescending(di => di.StatsDateTime))
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        CustomerExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetExistingAuthorization<Customer, CustomerExistingAuthorizationResponseDto>();

        return new CustomerDetailResponseDto(customer, authorizationResponseDto);
    }

    /// <inheritdoc />
    public async Task<Guid> CreateAsync(
            CustomerUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        Customer customer = new Customer
        {
            FirstName = requestDto.FirstName,
            MiddleName = requestDto.MiddleName,
            LastName = requestDto.LastName,
            NickName = requestDto.NickName,
            Gender = requestDto.Gender,
            Birthday = requestDto.Birthday,
            PhoneNumber = requestDto.PhoneNumber,
            ZaloNumber = requestDto.ZaloNumber,
            FacebookUrl = requestDto.FacebookUrl,
            Email = requestDto.Email,
            Address = requestDto.Address,
            CreatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            Note = requestDto.Note,
            IntroducerId = requestDto.IntroducerId,
            CreatedUserId = _authorizationService.GetUserId(),
        };

        context.Customers.Add(customer);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            await _summaryService.IncrementNewCustomerCountAsync(1, null, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return customer.Id;
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsForeignKeyConstraintViolation)
            {
                throw OperationException.NotFound(
                    new object[] { nameof(requestDto.IntroducerId) },
                    DisplayNames.Introducer
                );
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(
            Guid id,
            CustomerUpsertRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        Customer customer = await context.Customers
            .SingleOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new NotFoundException();

        customer.FirstName = requestDto.FirstName;
        customer.MiddleName = requestDto.MiddleName;
        customer.LastName = requestDto.LastName;
        customer.NickName = requestDto.NickName;
        customer.Gender = requestDto.Gender;
        customer.Birthday = requestDto.Birthday;
        customer.PhoneNumber = requestDto.PhoneNumber;
        customer.ZaloNumber = requestDto.ZaloNumber;
        customer.FacebookUrl = requestDto.FacebookUrl;
        customer.Email = requestDto.Email;
        customer.Address = requestDto.Address;
        customer.CreatedDateTime = DateTime.UtcNow.ToApplicationTime();
        customer.Note = requestDto.Note;
        customer.IntroducerId = requestDto.IntroducerId;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsForeignKeyConstraintViolation)
            {
                throw OperationException.NotFound(
                    new object[] { nameof(requestDto.IntroducerId) },
                    DisplayNames.Introducer
                );
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        await using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        IQueryable<Customer> query = context.Customers.Where(c => c.Id == id);
        DateTime lastUpdatedDateTime = DateTime.UtcNow.ToApplicationTime();

        bool isHardDeletionSuccessful = await TryExecuteDeleteAsync(
            async (token) =>
            {
                int updatedRecordCount = await query.ExecuteDeleteAsync(token);
                if (updatedRecordCount == 0)
                {
                    throw new NotFoundException();
                }
            },
            false,
            cancellationToken
        );

        if (!isHardDeletionSuccessful)
        {
            await TryExecuteDeleteAsync(
                async (token) =>
                {
                    int deletedRecordCount = await query.ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(c => c.LastUpdatedDateTime, lastUpdatedDateTime)
                            .SetProperty(c => c.IsDeleted, true),
                        token);

                    if (deletedRecordCount == 0)
                    {
                        throw new NotFoundException();
                    }
                },
                false,
                cancellationToken
            );
        }
        await transaction.CommitAsync(cancellationToken);
    }

    /// <inheritdoc />
    public ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions = new()
        {
            new()
            {
                Name = nameof(OrderByFieldOption.FirstName),
                DisplayName = DisplayNames.FirstName
            },
            new()
            {
                Name = nameof(OrderByFieldOption.Birthday),
                DisplayName = DisplayNames.Birthday
            },
            new()
            {
                Name = nameof(OrderByFieldOption.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            },
            new()
            {
                Name = nameof(OrderByFieldOption.DebtRemainingAmount),
                DisplayName = DisplayNames.DebtRemainingAmount
            },
            new()
            {
                Name = nameof(OrderByFieldOption.LastName),
                DisplayName = DisplayNames.LastName
            }
        };

        return new()
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Where(fo => fo.Name == nameof(OrderByFieldOption.LastName))
                .Select(fo => fo.Name)
                .Single(),
            DefaultAscending = true
        };
    }

    /// <inheritdoc />
    public bool GetCreatingPermission()
    {
        return _authorizationService.CanCreate<Customer>();
    }

    /// <inheritdoc />
    public async Task<NewCustomerCountResponseDto> GetNewCustomerSummaryThisMonthAsync(
            CancellationToken cancellationToken = default)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.ToApplicationTime());

        var result = await context.Customers
            .GroupBy(customer => 1)
            .Select(group => new
            {
                ThisMonthCount = group.Count(customer =>
                    customer.CreatedDateTime.Month == today.Month &&
                    customer.CreatedDateTime.Year == today.Year),
                LastMonthCount = group.Count(customer =>
                    customer.CreatedDateTime.Month == today.AddMonths(-1).Month &&
                    customer.CreatedDateTime.Year == today.AddMonths(-1).Year)
            }).FirstOrDefaultAsync(cancellationToken)
            ?? new
            {
                ThisMonthCount = 0,
                LastMonthCount = 0
            };

        decimal ratio;
        int ratioInPercentage;
        if (result.ThisMonthCount >= result.LastMonthCount)
        {
            ratio = (decimal)result.ThisMonthCount / result.LastMonthCount;
            ratioInPercentage = (int)Math.Round(ratio * 100) - 100;
        }
        else
        {
            ratio = -(decimal)result.LastMonthCount / result.ThisMonthCount;
            ratioInPercentage = (int)Math.Round(ratio * 100) + 100;
        }

        return new NewCustomerCountResponseDto
        {
            ThisMonthCount = result.ThisMonthCount,
            PercentageComparedToLastMonth = ratioInPercentage
        };
    }
    #endregion

    #region PrivateMethods
    private async Task<bool> TryExecuteDeleteAsync(
            Func<CancellationToken, Task> deleteAction,
            bool throwOnFailure,
            CancellationToken cancellationToken = default)
    {
        try
        {
            await deleteAction(cancellationToken);
            return true;
        }
        catch (DbException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsForeignKeyConstraintViolation)
            {
                if (throwOnFailure)
                {
                    throw OperationException.DeleteRestricted([], DisplayNames.Customer);
                }

                return false;
            }

            throw;
        }
    }
    #endregion
}