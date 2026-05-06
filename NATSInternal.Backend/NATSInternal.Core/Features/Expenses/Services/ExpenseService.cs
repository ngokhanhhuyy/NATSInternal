using JetBrains.Annotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
// using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Expenses;

[UsedImplicitly]
internal class ExpenseService : IExpenseService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<ExpenseListRequestDto> _listValidator;
    private readonly IValidator<ExpenseUpsertRequestDto> _upsertValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public ExpenseService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationService,
        IValidator<ExpenseListRequestDto> listValidator,
        IValidator<ExpenseUpsertRequestDto> upsertValidator,
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
    public async Task<ExpenseListResponseDto> GetListAsync(ExpenseListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Expense> query = _context.Expenses
            .Include(s => s.Photos.Where(photo => photo.IsThumbnail))
            .Where(s => s.DeletedDateTime == null);

        if (requestDto.StatsMonthYear is not null)
        {
            query = query.HasStatsMonthYear(requestDto.StatsMonthYear.Year, requestDto.StatsMonthYear.Month);
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(ExpenseListRequestDto.FieldToSort.StatsDateTime):
                query = query
                    .ApplySorting(e => e.StatsDate, requestDto.SortByAscending)
                    .ThenApplySorting(e => e.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(ExpenseListRequestDto.FieldToSort.CreatedDateTime):
                query = query
                    .ApplySorting(e => e.CreatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(e => e.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(ExpenseListRequestDto.FieldToSort.LastUpdatedDateTime):
                query = query
                    .ApplySorting(e => e.LastUpdatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(e => e.StatsDate, requestDto.SortByAscending);
                break;
            case nameof(ExpenseListRequestDto.FieldToSort.Amount):
                query = query
                    .ApplySorting(e => e.Amount, requestDto.SortByAscending)
                    .ThenApplySorting(e => e.StatsDate, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Expense> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<ExpenseBasicResponseDto> supplyResponseDtos = queryResult.Items
            .Select(e => new ExpenseBasicResponseDto(e, _authorizationService.GetExpenseExistingAuthorization(e)))
            .ToList();

        return new(supplyResponseDtos, queryResult.ItemCount, queryResult.PageCount);
    }
    
    public async Task<ExpenseDetailResponseDto> GetDetailAsync(int id)
    {
        Expense expense = await _context.Expenses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.Photos)
            .Include(s => s.CreatedUser)
            .Include(s => s.LastUpdatedUser)
            .Include(s => s.DeletedUser)
            .SingleOrDefaultAsync(e => e.Id == id)
            ?? throw new NotFoundException();

        ExpenseExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetExpenseExistingAuthorization(expense);

        return new(expense, authorization);
    }

    public async Task<int> CreateAsync(ExpenseUpsertRequestDto requestDto)
    {
        if (!_authorizationService.CanCreateExpense())
        {
            throw new AuthorizationException();
        }

        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet();
        });

        DateTime currentDateTime = _clock.Now;
        Expense expense = new()
        {
            StatsDate = requestDto.StatsDate ?? DateOnly.FromDateTime(currentDateTime),
            Amount = requestDto.Amount,
            Type = requestDto.Type,
            Note = requestDto.Note,
            CreatedDateTime = currentDateTime,
            CreatedUserId = _callerDetailProvider.GetId()
        };

        // TODO: Implement photo creation.

        _context.Expenses.Add(expense);
        
        try
        {
            await _context.SaveChangesAsync();
            return expense.Id;
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateException(exception);
            throw;
        }
    }

    public async Task UpdateAsync(int id, ExpenseUpsertRequestDto requestDto)
    {
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Update").IncludeRulesNotInRuleSet();
        });

        Expense expense = await _context.Expenses
            .Include(e => e.Photos)
            .SingleOrDefaultAsync(e => e.Id == id && e.DeletedDateTime == null)
            ?? throw new NotFoundException();

        ExpenseExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetExpenseExistingAuthorization(expense);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

        expense.StatsDate = requestDto.StatsDate ?? expense.StatsDate;
        expense.Amount = requestDto.Amount;
        expense.Type = requestDto.Type;
        expense.Note = requestDto.Note;
        expense.LastUpdatedDateTime = _clock.Now;
        expense.LastUpdatedUserId = _callerDetailProvider.GetId();

        // TODO: Implement photo creation and updation.

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateException(exception);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        Expense expense = await _context.Expenses
            .Include(e => e.Photos)
            .SingleOrDefaultAsync(e => e.Id == id && e.DeletedDateTime == null)
            ?? throw new NotFoundException();

        ExpenseExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetExpenseExistingAuthorization(expense);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        expense.DeletedDateTime = _clock.Now;
        expense.DeletedUserId = _callerDetailProvider.GetId();

        // TODO: Implement photo deletion.

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ThrowDbUpdateException(exception);
            throw;
        }
    }
    #endregion

    #region PrivateMethods
    private void ThrowDbUpdateException(DbUpdateException exception)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is null)
        {
            return;
        }

        if (handledResult.IsConcurrencyConflict || handledResult.IsForeignKeyConstraintViolation)
        {
            throw new ConcurrencyException();
        }
    }
    #endregion
}