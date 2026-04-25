using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Customers;

internal class CustomerService : ICustomerService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<CustomerListRequestDto> _listValidator;
    private readonly IValidator<CustomerUpsertRequestDto> _upsertValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public CustomerService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationService,
        IValidator<CustomerListRequestDto> listValidator,
        IValidator<CustomerUpsertRequestDto> upsertValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
        _exceptionHandler = exceptionHandler;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<CustomerListResponseDto> GetListAsync(CustomerListRequestDto requestDto)
    {
        requestDto.TransformValues();
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Customer> query = _context.Customers.Where(c => c.DeletedDateTime == null);

        if (!string.IsNullOrEmpty(requestDto.SearchContent))
        {
            string lowercasedSearchContent = requestDto.SearchContent.ToLower();
            query = query.Where(c => (
                c.FullName.ToLower().Contains(lowercasedSearchContent) ||
                (c.PhoneNumber != null && c.PhoneNumber.Contains(lowercasedSearchContent)) ||
                (c.ZaloNumber != null && c.ZaloNumber.Contains(lowercasedSearchContent)) ||
                (c.FacebookUrl != null && c.FacebookUrl.Contains(lowercasedSearchContent)) ||
                (c.Email != null && c.Email.Contains(lowercasedSearchContent)) ||
                (c.Address != null && c.Address.Contains(lowercasedSearchContent))
            ));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(CustomerListRequestDto.FieldToSort.LastName):
                query = query.ApplySorting(c => c.LastName, requestDto.SortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.FirstName):
                query = query.ApplySorting(c => c.FirstName, requestDto.SortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.Birthday):
                query = query.ApplySorting(c => c.Birthday, requestDto.SortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(c => c.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(CustomerListRequestDto.FieldToSort.DebtRemainingAmount):
                throw new NotImplementedException();
            default:
                throw new NotImplementedException();
        }

        Page<Customer> queryResult = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage
        );
        
        List<CustomerBasicResponseDto> customerResponseDtos = queryResult.Items
            .Select(u => new CustomerBasicResponseDto(u, _authorizationService.GetCustomerExistingAuthorization(u)))
            .ToList();
            
        return new(customerResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }
    
    public async Task<CustomerDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Customers
            .Where(c => c.Id == id)
            .Include(c => c.CreatedUser)
            .Include(c => c.LastUpdatedUser)
            .Include(c => c.DeletedUser)
            .Include(c => c.Introducer)
            .Select(c => new CustomerDetailResponseDto(c, _authorizationService.GetCustomerExistingAuthorization(c)))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }
    
    public async Task<int> CreateAsync(CustomerUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

        Customer customer = new()
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
            Address = requestDto.Email,
            Note = requestDto.Note,
            IntroducerId = requestDto.IntroducerId,
            CreatedDateTime = _clock.Now,
            CreatedUserId = _callerDetailProvider.GetId()
        };

        _context.Customers.Add(customer);

        try
        {
            await _context.SaveChangesAsync();
            return customer.Id;
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            object[] propertyPathElements;
            if (handledResult.IsForeignKeyConstraintViolation)
            {
                if (handledResult.ViolatedPropertyName == nameof(Customer.IntroducerId))
                {
                    propertyPathElements = new object[] { nameof(requestDto.IntroducerId) };
                    throw OperationException.NotFound(propertyPathElements, DisplayNames.Introducer);
                }

                if (handledResult.ViolatedPropertyName == nameof(Customer.CreatedUserId))
                {
                    throw new ConcurrencyException();
                }
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                propertyPathElements = new object[] { nameof(requestDto.NickName) };
                throw OperationException.Duplicated(propertyPathElements, DisplayNames.NickName);
            }

            throw;
        }
    }

    public async Task UpdateAsync(int id, CustomerUpsertRequestDto requestDto)
    {
        requestDto.TransformValues();
        _upsertValidator.ValidateAndThrow(requestDto);

        Customer customer = await _context.Customers
            .SingleOrDefaultAsync(c => c.Id == id && c.DeletedDateTime == null)
            ?? throw new NotFoundException();

        CustomerExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetCustomerExistingAuthorization(customer);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

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
        customer.Address = requestDto.Email;
        customer.Note = requestDto.Note;
        customer.IntroducerId = requestDto.IntroducerId;
        customer.LastUpdatedDateTime = _clock.Now;
        customer.LastUpdatedUserId = _callerDetailProvider.GetId();

        try
        {
            await _context.SaveChangesAsync();
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

            object[] propertyPathElements;
            if (handledResult.IsForeignKeyConstraintViolation && handledResult.ViolatedPropertyName is not null)
            {
                string violatedPropertyName = handledResult.ViolatedPropertyName;
                if (violatedPropertyName == nameof(Customer.IntroducerId))
                {
                    propertyPathElements = new object[] { nameof(requestDto.IntroducerId) };
                    throw OperationException.NotFound(propertyPathElements, DisplayNames.Introducer);
                }

                if (violatedPropertyName is nameof(Customer.CreatedUserId) or nameof(Customer.LastUpdatedUserId))
                {
                    throw new ConcurrencyException();
                }
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                propertyPathElements = new object[] { nameof(requestDto.NickName) };
                throw OperationException.Duplicated(propertyPathElements, DisplayNames.NickName);
            }

            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        Customer customer = await _context.Customers
            .SingleOrDefaultAsync(c => c.Id == id && c.DeletedDateTime == null)
            ?? throw new NotFoundException();

        CustomerExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetCustomerExistingAuthorization(customer);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        customer.DeletedDateTime = _clock.Now;
        customer.DeletedUserId = _callerDetailProvider.GetId();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict || handledResult.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    #endregion
}
