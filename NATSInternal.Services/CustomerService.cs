namespace NATSInternal.Services;

/// <inheritdoc />
internal class CustomerService
    :
        UpsertableAbstractService<Customer, CustomerListRequestDto>,
        ICustomerService
{
    private readonly DatabaseContext _context;
    private readonly IAuthorizationInternalService _authorizationService;

    public CustomerService(
            DatabaseContext context,
            IAuthorizationInternalService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    public async Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto)
    {
        // Initialize query.
        IQueryable<Customer> query = _context.Customers
            .Include(c => c.DebtIncurrences)
            .Include(c => c.DebtPayments);

        // Determine the field and the direction the sort.
        switch (requestDto.OrderByField)
        {
            case nameof(OrderByFieldOptions.FirstName):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(c => c.FirstName)
                    : query.OrderByDescending(c => c.FirstName);
                break;
            case nameof(OrderByFieldOptions.Birthday):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(c => c.Birthday)
                    : query.OrderByDescending(c => c.Birthday);
                break;
            case nameof(OrderByFieldOptions.CreatedDateTime):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(c => c.CreatedDateTime)
                    : query.OrderByDescending(c => c.CreatedDateTime);
                break;
            case nameof(OrderByFieldOptions.DebtRemainingAmount):
                query = requestDto.OrderByAscending
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
            case nameof(OrderByFieldOptions.LastName):
                query = requestDto.OrderByAscending
                    ? query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName)
                    : query.OrderByDescending(c => c.LastName)
                        .ThenByDescending(c => c.FirstName);
                break;
            default:
                throw new NotImplementedException();

        }

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
                .Where(d => !d.IsDeleted)
                .Sum(d => d.Amount) > c.DebtPayments
                .Where(dp => !dp.IsDeleted)
                .Sum(dp => dp.Amount));
        }

        // Fetch the list of the entities.
        EntityListDto<Customer> listDto = await GetListOfEntitiesAsync(query, requestDto);

        return new CustomerListResponseDto
        {
            PageCount = listDto.PageCount,
            Items = listDto.Items
                .Select(customer => new CustomerBasicResponseDto(
                    customer,
                    _authorizationService.GetCustomerAuthorization(customer)))
                .ToList(),
            Authorization = _authorizationService.GetCustomerListAuthorization()
        };
    }

    /// <inheritdoc />
    public async Task<CustomerBasicResponseDto> GetBasicAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.DebtIncurrences)
            .Include(c => c.DebtPayments)
            .Where(c => c.Id == id)
            .Select(c => new CustomerBasicResponseDto(
                c,
                _authorizationService.GetCustomerAuthorization(c)))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Customer),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<CustomerDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Customers
            .Include(customer => customer.Introducer)
            .Include(customer => customer.CreatedUser)
            .Include(customer => customer.DebtIncurrences)
            .Include(customer => customer.DebtPayments)
            .Where(c => !c.IsDeleted && c.Id == id)
            .Select(customer => new CustomerDetailResponseDto(
                customer,
                _authorizationService))
            .SingleOrDefaultAsync()
            ?? throw new ResourceNotFoundException(
                nameof(Customer),
                nameof(id),
                id.ToString());
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CustomerUpsertRequestDto requestDto)
    {
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
            CreatedUserId = _authorizationService.GetUserId()
        };
        _context.Customers.Add(customer);
        try
        {
            await _context.SaveChangesAsync();
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
        string fullName = PersonNameUtility.GetFullNameFromNameElements(
            requestDto.FirstName,
            requestDto.MiddleName,
            requestDto.LastName);

        try
        {
            int affactedRows = await _context.Customers
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
            
            // Check if the customer has been updated.
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
        int affectedRows = await _context.Customers
            .Where(c => !c.IsDeleted && c.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.UpdatedDateTime, DateTime.UtcNow.ToApplicationTime())
                .SetProperty(c => c.IntroducerId, (int?)null)
                .SetProperty(c => c.IsDeleted, true));

        if (affectedRows == 0)
        {
            throw new ResourceNotFoundException(
                nameof(Customer),
                nameof(id),
                id.ToString());
        }
    }
}
