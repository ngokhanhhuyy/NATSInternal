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
using Microsoft.EntityFrameworkCore.Storage;

namespace NATSInternal.Core.Features.Debts;

[UsedImplicitly]
internal class DebtInternalService : IDebtInternalService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly ICustomerInternalService _customerService;
    private readonly IValidator<DebtListRequestDto> _listValidator;
    private readonly IValidator<DebtCreateRequestDto> _createValidator;
    private readonly IValidator<DebtUpdateRequestDto> _updateValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public DebtInternalService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationService,
        ICustomerInternalService customerService,
        IValidator<DebtListRequestDto> listValidator,
        IValidator<DebtCreateRequestDto> createValidator,
        IValidator<DebtUpdateRequestDto> updateValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
        _customerService = customerService;
        _listValidator = listValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _exceptionHandler = exceptionHandler;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<DebtListResponseDto> GetListAsync(DebtListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Debt> query = _context.Debts
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
            case nameof(DebtListRequestDto.FieldToSort.StatsDate):
                query = query
                    .ApplySorting(p => p.StatsDate, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(DebtListRequestDto.FieldToSort.CreatedDateTime):
                query = query
                    .ApplySorting(p => p.CreatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(DebtListRequestDto.FieldToSort.LastUpdatedDateTime):
                query = query
                    .ApplySorting(p => p.LastUpdatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(DebtListRequestDto.FieldToSort.Amount):
                query = query
                    .ApplySorting(p => p.Amount, requestDto.SortByAscending)
                    .ThenApplySorting(p => p.StatsDate, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Debt> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<DebtBasicResponseDto> supplyResponseDtos = queryResult.Items
            .Select(d => new DebtBasicResponseDto(d, _authorizationService.GetDebtExistingAuthorization(d)))
            .ToList();

        return new(supplyResponseDtos, queryResult.ItemCount, queryResult.PageCount);
    }

    public async Task<DebtDetailResponseDto> GetDetailAsync(int id)
    {
        Debt debt = await _context.Debts
            .Include(d => d.Customer)
            .Include(d => d.CreatedUser)
            .Include(d => d.LastUpdatedUser)
            .Include(d => d.DeletedUser)
            .SingleOrDefaultAsync(d => d.Id == id)
            ?? throw new NotFoundException();

        DebtExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetDebtExistingAuthorization(debt);

        return new(debt, authorization);
    }

    public async Task<int> CreateAsync(DebtCreateRequestDto requestDto)
    {
        if (!_authorizationService.CanCreateDebt())
        {
            throw new AuthorizationException();
        }

        _createValidator.ValidateAndThrow(requestDto);

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        DateTime currentDateTime = _clock.Now;
        Debt debt = new()
        {
            Amount = requestDto.Amount,
            StatsDate = requestDto.StatsDate ?? DateOnly.FromDateTime(currentDateTime),
            Note = requestDto.Note,
            CustomerId = requestDto.CustomerId,
            CreatedDateTime = currentDateTime,
            CreatedUserId = _callerDetailProvider.GetId()
        };

        _context.Debts.Add(debt);

        try
        {
            await _context.SaveChangesAsync();

            long UpdateCachedDebtAmount(long currentAmount) => currentAmount + requestDto.Amount;
            await _customerService.UpdateCachedRemaningDebtAmountAsync(requestDto.CustomerId, UpdateCachedDebtAmount);

            return debt.Id;
        }
        catch (DbUpdateException exception)
        {
            ThrowFromDbUpdateException(exception, isWhenCreating: true);
            throw;
        }
        catch (NotFoundException)
        {
            throw new ConcurrencyException();
        }
    }

    public async Task UpdateAsync(int id, DebtUpdateRequestDto requestDto)
    {
        _updateValidator.ValidateAndThrow(requestDto);

        Debt debt = await _context.Debts
            .Include(d => d.Customer)
            .Where(d => d.Id == id)
            .Where(d => d.DeletedDateTime == null)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();

        DebtExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetDebtExistingAuthorization(debt);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

        long amountDiff = requestDto.Amount - debt.Amount;
        if (debt.Customer.CachedDebtRemainingAmount + amountDiff < 0)
        {
            object[] propertyPathElements = new object[] { nameof(requestDto.Amount) };
            string errorMessage = ErrorMessages.NegativeRemainingDebtAmount;
            throw new OperationException(propertyPathElements, errorMessage);
        }

        debt.StatsDate = requestDto.StatsDate ?? debt.StatsDate;
        debt.Amount = requestDto.Amount;
        debt.Note = requestDto.Note;
        debt.LastUpdatedDateTime = _clock.Now;
        debt.LastUpdatedUserId = _callerDetailProvider.GetId();

        await using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            await _context.SaveChangesAsync();

            long UpdateCachedDebtAmount(long currentAmount) => currentAmount + amountDiff;
            await _customerService.UpdateCachedRemaningDebtAmountAsync(debt.Customer, UpdateCachedDebtAmount);
        }
        catch (DbUpdateException exception)
        {
            ThrowFromDbUpdateException(exception, isWhenCreating: false);
            throw;
        }
        catch (NotFoundException)
        {
            throw new ConcurrencyException();
        }
    }

    public async Task DeleteAsync(int id)
    {
        Debt debt = await _context.Debts
            .Include(d => d.Customer)
            .SingleOrDefaultAsync(d => d.Id == id && d.DeletedDateTime == null)
            ?? throw new NotFoundException();

            
    }
    #endregion

    #region PrivateMethods
    private void ThrowFromDbUpdateException(DbUpdateException exception, bool isWhenCreating)
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
            if (isWhenCreating && handledResult.ViolatedPropertyName is nameof(Debt.CustomerId))
            {
                object[] propertyPathElements = new[] { nameof(DebtCreateRequestDto.CustomerId) };
                throw OperationException.NotFound(propertyPathElements, DisplayNames.Customer);
            }

            throw new ConcurrencyException();
        }
    }
    #endregion
}
