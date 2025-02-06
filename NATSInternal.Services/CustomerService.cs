namespace NATSInternal.Services;

/// <inheritdoc cref="ICustomerService" />
internal class CustomerService
    :
        UpsertableAbstractService<
            Customer,
            CustomerListRequestDto,
            CustomerExistingAuthorizationResponseDto>,
        ICustomerService
{
    private readonly IDbContextFactory<DatabaseContext> _contextFactory;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IStatsInternalService _statsService;

    public CustomerService(
            IDbContextFactory<DatabaseContext> contextFactory,
            IAuthorizationInternalService authorizationService,
            IStatsInternalService statsService) : base(authorizationService)
    { 
        _contextFactory = contextFactory;
        _authorizationService = authorizationService;
        _statsService = statsService;
    }

    public async Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        
        // Initialize query.
        IQueryable<Customer> query = context.Customers
            .Include(c => c.CreatedUser).ThenInclude(u => u.Roles)
            .Include(c => c.DebtIncurrences)
            .Include(c => c.DebtPayments);

        // Filter by search content.
        if (requestDto.SearchByContent != null)
        {
            bool isValidBirthday = DateOnly.TryParse(
                requestDto.SearchByContent,
                out DateOnly birthday);
            query = query.Where(c =>
                c.NormalizedFullName
                    .ToLower()
                    .Contains(requestDto.SearchByContent.ToLower()) ||
                c.PhoneNumber.Contains(requestDto.SearchByContent) ||
                (isValidBirthday && c.Birthday.HasValue && c.Birthday.Value == birthday));
        }

        // Filter by remaining debt amount.
        if (requestDto.HasRemainingDebtAmountOnly)
        {
            query = query.Where(c => c.DebtIncurrences
                .Where(di => !di.IsDeleted)
                .Sum(di => di.Amount) - c.DebtPayments
                .Where(dp => !dp.IsDeleted)
                .Sum(dp => dp.Amount) > 0);
        }

        // Determine the field and the direction the sort.
        string sortingByField = requestDto.SortingByField
                                ?? GetListSortingOptions().DefaultFieldName;
        bool sortingByAscending = requestDto.SortingByAscending
                                  ?? GetListSortingOptions().DefaultAscending;
        switch (sortingByField)
        {
            case nameof(OrderByFieldOption.FirstName):
                query = sortingByAscending
                    ? query.OrderBy(c => c.FirstName)
                    : query.OrderByDescending(c => c.FirstName);
                break;
            case nameof(OrderByFieldOption.Birthday):
                query = sortingByAscending
                    ? query.OrderBy(c => c.Birthday)
                    : query.OrderByDescending(c => c.Birthday);
                break;
            case nameof(OrderByFieldOption.CreatedDateTime):
                query = sortingByAscending
                    ? query.OrderBy(c => c.CreatedDateTime)
                    : query.OrderByDescending(c => c.CreatedDateTime);
                break;
            case nameof(OrderByFieldOption.DebtRemainingAmount):
                query = sortingByAscending
                    ? query.OrderBy(c => c.DebtIncurrences
                            .Where(d => !d.IsDeleted)
                            .Sum(d => d.Amount) - c.DebtPayments
                            .Where(dp => !dp.IsDeleted)
                            .Sum(dp => dp.Amount))
                        .ThenBy(c => c.Id)
                    : query.OrderByDescending(c => c.DebtIncurrences
                            .Where(d => !d.IsDeleted)
                            .Sum(d => d.Amount) - c.DebtPayments
                            .Where(dp => !dp.IsDeleted)
                            .Sum(dp => dp.Amount))
                        .ThenByDescending(c => c.Id);
                break;
            case nameof(OrderByFieldOption.LastName):
                query = sortingByAscending
                    ? query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                    : query.OrderByDescending(c => c.LastName)
                        .ThenByDescending(c => c.FirstName);
                break;
            default:
                throw new NotImplementedException();

        }

        // Fetch the list of the entities.
        EntityListDto<Customer> listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new CustomerListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items?
                .Select(c => new CustomerBasicResponseDto(c, GetExistingAuthorization(c)))
                .ToList()
        };
    }

    /// <inheritdoc />
    public async Task<CustomerBasicResponseDto> GetBasicAsync(int id)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Customers
            .Include(c => c.DebtIncurrences)
            .Include(c => c.DebtPayments)
            .Where(c => c.Id == id)
            .Select(c => new CustomerBasicResponseDto(c, GetExistingAuthorization(c)))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Customer),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<CustomerDetailResponseDto> GetDetailAsync(int id)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        
        Customer customer = await context.Customers
            .Include(customer => customer.Introducer)
            .Include(customer => customer.CreatedUser).ThenInclude(u => u.Roles)
            .Include(customer => customer.DebtIncurrences
                .OrderByDescending(di => di.StatsDateTime))
            .Include(customer => customer.DebtPayments
                .OrderByDescending(dp => dp.StatsDateTime))
            .Where(c => !c.IsDeleted && c.Id == id)
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Customer),
                nameof(id),
                id.ToString());

        return new CustomerDetailResponseDto(
            customer,
            GetExistingAuthorization(customer),
            _authorizationService.GetExistingAuthorization<
                DebtIncurrence,
                DebtIncurrenceUpdateHistory,
                DebtIncurrenceExistingAuthorizationResponseDto>,
            _authorizationService.GetExistingAuthorization<
                DebtPayment,
                DebtPaymentUpdateHistory,
                DebtPaymentExistingAuthorizationResponseDto>);
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CustomerUpsertRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        await using IDbContextTransaction transaction = await context.Database
            .BeginTransactionAsync();
        
        string fullName = PersonNameUtility.GetFullNameFromNameElements(
            requestDto.FirstName,
            requestDto.MiddleName,
            requestDto.LastName);

        Customer customer = new Customer
        {
            FirstName = requestDto.FirstName,
            NormalizedFirstName = requestDto.FirstName.ToNonDiacritics().ToUpper(),
            MiddleName = requestDto.MiddleName,
            NormalizedMiddleName = requestDto.MiddleName?.ToNonDiacritics().ToUpper(),
            LastName = requestDto.LastName,
            NormalizedLastName = requestDto.LastName?.ToNonDiacritics().ToUpper(),
            FullName = fullName,
            NormalizedFullName = fullName.ToNonDiacritics().ToUpper(),
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
            IntroducerId = null,
            CreatedUserId = _authorizationService.GetUserId(),
        };
        context.Customers.Add(customer);
        
        try
        {
            await context.SaveChangesAsync();
            await _statsService.IncrementNewCustomerAsync(1);
            await transaction.CommitAsync();
            return customer.Id;
        }
        catch (DbUpdateException exception)
        {
            if (exception.InnerException is MySqlException sqlException)
            {
                SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
                if (exceptionHandler.IsForeignKeyNotFound)
                {
                    throw new OperationException(
                        nameof(requestDto.IntroducerId),
                        ErrorMessages.NotFound.ReplaceResourceName(DisplayNames.Introducer));
                }
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateAsync(int id, CustomerUpsertRequestDto requestDto)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        
        string fullName = PersonNameUtility.GetFullNameFromNameElements(
            requestDto.FirstName,
            requestDto.MiddleName,
            requestDto.LastName);

        try
        {
            int affactedRows = await context.Customers
                .Where(c => !c.IsDeleted && c.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.FirstName, requestDto.FirstName)
                    .SetProperty(c => c.NormalizedFirstName, requestDto.FirstName
                        .ToNonDiacritics()
                        .ToUpper())
                    .SetProperty(c => c.MiddleName, requestDto.MiddleName)
                    .SetProperty(c => c.NormalizedMiddleName, requestDto.MiddleName
                        .ToNonDiacritics()
                        .ToUpper())
                    .SetProperty(c => c.LastName, requestDto.LastName)
                    .SetProperty(c => c.NormalizedLastName, requestDto.LastName
                        .ToNonDiacritics()
                        .ToUpper())
                    .SetProperty(c => c.FullName, fullName)
                    .SetProperty(c => c.NormalizedFullName, fullName
                        .ToNonDiacritics()
                        .ToUpper())
                    .SetProperty(c => c.NickName, requestDto.NickName)
                    .SetProperty(c => c.Gender, requestDto.Gender)
                    .SetProperty(c => c.Birthday, requestDto.Birthday)
                    .SetProperty(c => c.PhoneNumber, requestDto.PhoneNumber)
                    .SetProperty(c => c.ZaloNumber, requestDto.ZaloNumber)
                    .SetProperty(c => c.FacebookUrl, requestDto.FacebookUrl)
                    .SetProperty(c => c.Email, requestDto.Email)
                    .SetProperty(c => c.Address, requestDto.Address)
                    .SetProperty(c => c.UpdatedDateTime, DateTime.UtcNow.ToApplicationTime())
                    .SetProperty(c => c.Note, requestDto.Note)
                    .SetProperty(c => c.IntroducerId, requestDto.IntroducerId));
            
            // Check if the c has been updated.
            if (affactedRows == 0)
            {
                throw new ResourceNotFoundException(
                    nameof(Customer),
                    nameof(id),
                    id.ToString());
            }
        }
        catch (MySqlException sqlException)
        {
            SqlExceptionHandler exceptionHandler = new SqlExceptionHandler(sqlException);
            if (exceptionHandler.IsForeignKeyNotFound)
            {
                throw new OperationException(
                    nameof(requestDto.IntroducerId),
                    ErrorMessages.NotFound.ReplaceResourceName(DisplayNames.Introducer));
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await using DatabaseContext context = await _contextFactory.CreateDbContextAsync();
        await using IDbContextTransaction transaction = await context.Database
            .BeginTransactionAsync();
        
        int affectedRows = await context.Customers
            .Where(c => !c.IsDeleted && c.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.UpdatedDateTime, DateTime.UtcNow.ToApplicationTime())
                .SetProperty(c => c.IntroducerId, (int?)null)
                .SetProperty(c => c.IsDeleted, true));

        if (affectedRows != 1)
        {
            throw new ResourceNotFoundException(
                nameof(Customer),
                nameof(id),
                id.ToString());
        }

        await _statsService.IncrementNewCustomerAsync(-1);
        await transaction.CommitAsync();
    }
    
    /// <inheritdoc cref="ICustomerService.GetListSortingOptions" />
    public override ListSortingOptionsResponseDto GetListSortingOptions()
    {
        List<ListSortingByFieldResponseDto> fieldOptions;
        fieldOptions = new List<ListSortingByFieldResponseDto>
        {
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.FirstName),
                DisplayName = DisplayNames.FirstName
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.Birthday),
                DisplayName = DisplayNames.Birthday
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.CreatedDateTime),
                DisplayName = DisplayNames.CreatedDateTime
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.DebtRemainingAmount),
                DisplayName = DisplayNames.DebtRemainingAmount
            },
            new ListSortingByFieldResponseDto
            {
                Name = nameof(OrderByFieldOption.LastName),
                DisplayName = DisplayNames.LastName
            }
        };

        return new ListSortingOptionsResponseDto
        {
            FieldOptions = fieldOptions,
            DefaultFieldName = fieldOptions
                .Single(i => i.Name == nameof(OrderByFieldOption.LastName))
                .Name,
            DefaultAscending = true
        };
    }

    /// <inheritdoc cref="ICustomerService.GetNewStatsAsync" />
    public async Task<NewCustomerCountResponseDto> GetNewStatsAsync()
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
            }).FirstOrDefaultAsync()
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
}
