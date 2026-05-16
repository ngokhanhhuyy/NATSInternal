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
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Payments;

[UsedImplicitly]
internal class PaymentInternalService : IPaymentInternalService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<PaymentListRequestDto> _listValidator;
    private readonly IValidator<PaymentCreateRequestDto> _createValidator;
    private readonly IValidator<PaymentUpdateRequestDto> _updateValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public PaymentInternalService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationService,
        IValidator<PaymentListRequestDto> listValidator,
        IValidator<PaymentCreateRequestDto> createValidator,
        IValidator<PaymentUpdateRequestDto> updateValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _exceptionHandler = exceptionHandler;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<PaymentListResponseDto> GetListAsync(PaymentListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Payment> query = _context.Payments
            .Include(p => p.Customer)
            .Where(p => p.DeletedDateTime == null);

        if (requestDto.StatsMonthYear is not null)
        {
            query = query.HasStatsMonthYear(requestDto.StatsMonthYear.Year, requestDto.StatsMonthYear.Month);
        }

        if (requestDto.CustomerId.HasValue)
        {
            query = query.Where(p => p.CustomerId == requestDto.CustomerId.Value);
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(PaymentListRequestDto.FieldToSort.StatsDate):
                query = query
                    .ApplySorting(p => p.StatsDate, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(PaymentListRequestDto.FieldToSort.CreatedDateTime):
                query = query
                    .ApplySorting(p => p.CreatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(PaymentListRequestDto.FieldToSort.LastUpdatedDateTime):
                query = query
                    .ApplySorting(p => p.LastUpdatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(PaymentListRequestDto.FieldToSort.Amount):
                query = query
                    .ApplySorting(p => p.Amount, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.StatsDate, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Payment> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<PaymentBasicResponseDto> supplyResponseDtos = queryResult.Items
            .Select(p => new PaymentBasicResponseDto(
                p,
                _authorizationService.GetPaymentExistingAuthorization(p)))
            .ToList();

        return new(supplyResponseDtos, queryResult.ItemCount, queryResult.PageCount);
    }

    public async Task<PaymentDetailResponseDto> GetDetailAsync(int id)
    {
        Payment payment = await _context.Payments
            .Include(p => p.Customer)
            .Include(p => p.CreatedUser)
            .Include(p => p.LastUpdatedUser)
            .Include(p => p.DeletedUser)
            .SingleOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException();

        PaymentExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetPaymentExistingAuthorization(payment);

        return new(payment, authorization);
    }
    
    public async Task<int> CreateAsync(PaymentCreateRequestDto requestDto)
    {
        if (!_authorizationService.CanCreatePayment())
        {
            throw new AuthorizationException();
        }

        _createValidator.ValidateAndThrow(requestDto);

        requestDto.Type = PaymentType.DebtPayment;
        return await CreateWithoutValidationAsync(requestDto);
    }

    public async Task<int> CreateWithoutValidationAsync(PaymentCreateRequestDto requestDto)
    {
        if (!_authorizationService.CanCreatePayment())
        {
            throw new AuthorizationException();
        }

        Customer? customer = await _context.Customers
            .Where(c => c.Id == requestDto.CustomerId)
            .Where(c => c.DeletedDateTime != null)
            .SingleOrDefaultAsync();

        if (customer is null)
        {
            object[] propertyPathElements = new object[] { nameof(requestDto.CustomerId) };
            throw OperationException.NotFound(propertyPathElements, DisplayNames.Customer);
        }

        if (customer.CachedDebtAmount - requestDto.Amount < 0)
        {
            object[] propertyPathElements = new object[] { nameof(requestDto.Amount) };
            throw new OperationException(propertyPathElements, ErrorMessages.PaidAmountIsGreaterThanRemainingDebtAmount);
        }

        DateTime currentDateTime = _clock.Now;
        Payment payment = new()
        {
            StatsDate = requestDto.StatsDate ?? DateOnly.FromDateTime(currentDateTime),
            Type = requestDto.Type,
            Amount = requestDto.Amount,
            Note = requestDto.Note,
            CustomerId = customer.Id,
            Customer = customer,
            OrderId = requestDto.OrderId,
            CreatedDateTime = currentDateTime,
            CreatedUserId = _callerDetailProvider.GetId()
        };

        _context.Payments.Add(payment);

        try
        {
            await _context.SaveChangesAsync();
            return payment.Id;
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateException(exception, isForCreating: true);
            throw;
        }
    }

    public async Task UpdateAsync(int id, PaymentUpdateRequestDto requestDto)
    {
        _updateValidator.ValidateAndThrow(requestDto);

        Payment payment = await _context.Payments
            .SingleOrDefaultAsync(p => p.Id == id && p.DeletedDateTime == null)
            ?? throw new NotFoundException();

        await UpdateWithoutValidationAsync(payment, requestDto);
    }
    
    public async Task UpdateWithoutValidationAsync(Payment payment, PaymentUpdateRequestDto requestDto)
    {
        PaymentExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetPaymentExistingAuthorization(payment);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

        long amountDiff = requestDto.Amount - payment.Amount;
        if (payment.Customer.CachedDebtAmount - amountDiff < 0)
        {
            object[] propertyPathElements = new object[] { nameof(requestDto.Amount) };
            string errorMessage = ErrorMessages.PaidAmountIsGreaterThanRemainingDebtAmount;
            throw new OperationException(propertyPathElements, errorMessage);
        }

        payment.StatsDate = requestDto.StatsDate ?? payment.StatsDate;
        payment.Amount = requestDto.Amount;
        payment.Note = requestDto.Note;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateException(exception, isForCreating: false);
            throw;
        }
    }

    public async Task DeleteAsync(Payment payment)
    {
        if (payment.DeletedDateTime is not null)
        {
            return;
        }
        
        PaymentExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetPaymentExistingAuthorization(payment);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        payment.DeletedDateTime = _clock.Now;
        payment.DeletedUserId = _callerDetailProvider.GetId();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateException(exception, isForCreating: false);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        Payment payment = await _context.Payments
            .SingleOrDefaultAsync(p => p.Id == id && p.DeletedDateTime == null)
            ?? throw new NotFoundException();

        await DeleteAsync(payment);
    }
    #endregion

    #region PrivateMethods
    private void ThrowDbUpdateException(DbUpdateException exception, bool isForCreating)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is null)
        {
            return;
        }

        if (handledResult.IsConcurrencyConflict)
        {
            throw new ConcurrencyException();
        }

        if (handledResult.IsForeignKeyConstraintViolation)
        {
            if (isForCreating is not true)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.ViolatedPropertyName == nameof(Payment.CustomerId))
            {
                throw OperationException.NotFound(
                    new object[] { nameof(PaymentCreateRequestDto.CustomerId) },
                    DisplayNames.Customer
                );
            }

            if (handledResult.ViolatedPropertyName == nameof(Payment.OrderId))
            {
                throw OperationException.NotFound(
                    new object[] { nameof(PaymentCreateRequestDto.OrderId) },
                    DisplayNames.Customer
                );
            }
        }
    }
    #endregion
}
