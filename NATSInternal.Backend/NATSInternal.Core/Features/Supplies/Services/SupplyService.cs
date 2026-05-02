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

namespace NATSInternal.Core.Features.Supplies;

internal class SupplyService : ISupplyService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IHasProductService<SupplyUpsertItemRequestDto, SupplyItem> _hasProductService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<SupplyListRequestDto> _listValidator;
    private readonly IValidator<SupplyUpsertRequestDto> _upsertValidator;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public SupplyService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IHasProductService<SupplyUpsertItemRequestDto, SupplyItem> hasProductService,
        IAuthorizationInternalService authorizationService,
        IValidator<SupplyListRequestDto> listValidator,
        IValidator<SupplyUpsertRequestDto> upsertValidator,
        IDbExceptionHandler exceptionHandler,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _hasProductService = hasProductService;
        _authorizationService = authorizationService;
        _listValidator = listValidator;
        _upsertValidator = upsertValidator;
        _exceptionHandler = exceptionHandler;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion

    #region Methods
    public async Task<SupplyListResponseDto> GetListAsync(SupplyListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<Supply> query = _context.Supplies
            .Include(s => s.Photos.Where(photo => photo.IsThumbnail))
            .Where(s => s.DeletedDateTime == null);

        if (requestDto.StatsMonthYear is not null)
        {
            query = query.HasStatsMonthYear(requestDto.StatsMonthYear.Year, requestDto.StatsMonthYear.Month);
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(SupplyListRequestDto.FieldToSort.StatsDateTime):
                query = query
                    .ApplySorting(s => s.StatsDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(SupplyListRequestDto.FieldToSort.CreatedDateTime):
                query = query
                    .ApplySorting(s => s.CreatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDateTime, requestDto.SortByAscending);
                break;
            case nameof(SupplyListRequestDto.FieldToSort.LastUpdatedDateTime):
                query = query
                    .ApplySorting(s => s.LastUpdatedDateTime, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDateTime, requestDto.SortByAscending);
                break;
            case nameof(SupplyListRequestDto.FieldToSort.ItemAmount):
                query = query
                    .ApplySorting(s => s.CachedItemsAmount, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDateTime, requestDto.SortByAscending);
                break;
            case nameof(SupplyListRequestDto.FieldToSort.TotalAmount):
                query = query
                    .ApplySorting(s => s.CachedAmount, requestDto.SortByAscending)
                    .ThenApplySorting(s => s.StatsDateTime, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        (int page, int resultPerPage) = (requestDto.Page, requestDto.ResultsPerPage);
        Page<Supply> queryResult = await _listFetchingService.GetPagedListAsync(query, page, resultPerPage);

        List<SupplyBasicResponseDto> supplyResponseDtos = queryResult.Items
            .Select(s => new SupplyBasicResponseDto(s, _authorizationService.GetSupplyExistingAuthorization(s)))
            .ToList();

        return new(supplyResponseDtos, queryResult.ItemCount, queryResult.PageCount);
    }

    public async Task<SupplyDetailResponseDto> GetDetailAsync(int id)
    {
        Supply supply = await _context.Supplies
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.Items).ThenInclude(si => si.Product)
            .Include(s => s.Photos)
            .Include(s => s.CreatedUser)
            .Include(s => s.LastUpdatedUser)
            .Include(s => s.DeletedUser)
            .SingleOrDefaultAsync(s => s.Id == id)
            ?? throw new NotFoundException();

        SupplyExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetSupplyExistingAuthorization(supply);

        return new(supply, authorization);
    }

    public async Task<int> CreateAsync(SupplyUpsertRequestDto requestDto)
    {
        if (!_authorizationService.CanCreateSupply())
        {
            throw new AuthorizationException();
        }

        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("Create").IncludeRulesNotInRuleSet();
        });

        DateTime currentDateTime = _clock.Now;
        Supply supply = new()
        {
            StatsDateTime = requestDto.StatsDateTime ?? currentDateTime,
            ShipmentFee = requestDto.ShipmentFee,
            Note = requestDto.Note,
            CreatedDateTime = currentDateTime,
            CreatedUserId = _callerDetailProvider.GetId()
        };

        _context.Supplies.Add(supply);

        List<SupplyItem> supplyItems = await _hasProductService.CreateItemsAsync(requestDto.Items, MapItem);

        supply.Items.AddRange(supplyItems);

        // TODO: Handle photo creation.

        supply.ComputeCachedProperties();

        try
        {
            await _context.SaveChangesAsync();
            return supply.Id;
        }
        catch (DbUpdateException exception)
        {
            ConvertAndThrowException(exception);
            throw;
        }
    }

    public async Task UpdateAsync(int id, SupplyUpsertRequestDto requestDto)
    {
        _upsertValidator.Validate(requestDto, options =>
        {
            options.ThrowOnFailures();
            options.IncludeRuleSets("CreateOrUpdate").IncludeRulesNotInRuleSet();
        });

        Supply supply = await _context.Supplies
            .Include(s => s.Items).ThenInclude(si => si.Product)
            .Include(s => s.Photos)
            .SingleOrDefaultAsync(s => s.Id == id && s.DeletedDateTime == null)
            ?? throw new NotFoundException();

        SupplyExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetSupplyExistingAuthorization(supply);
        if (!authorization.CanEdit)
        {
            throw new AuthorizationException();
        }

        supply.StatsDateTime = requestDto.StatsDateTime ?? supply.StatsDateTime;
        supply.ShipmentFee = requestDto.ShipmentFee;
        supply.Note = requestDto.Note;
        supply.LastUpdatedDateTime = _clock.Now;
        supply.LastUpdatedUserId = _callerDetailProvider.GetId();

        await _hasProductService.UpdateItemsAsync(requestDto.Items, supply.Items, MapItem);

        // TODO: Handle photo creation or updation.

        supply.ComputeCachedProperties();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ConvertAndThrowException(exception);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        Supply supply = await _context.Supplies
            .SingleOrDefaultAsync(s => s.Id == id && s.DeletedDateTime == null)
            ?? throw new NotFoundException();

        SupplyExistingAuthorizationResponseDto authorization;
        authorization = _authorizationService.GetSupplyExistingAuthorization(supply);
        if (!authorization.CanDelete)
        {
            throw new AuthorizationException();
        }

        supply.DeletedDateTime = _clock.Now;
        supply.DeletedUserId = _callerDetailProvider.GetId();

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            ConvertAndThrowException(exception);
            throw;
        }
    }
    #endregion

    #region PrivateMethods
    private void ConvertAndThrowException(DbUpdateException exception)
    {
        DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
        if (handledResult is null)
        {
            return;
        }

        bool isConcurrencyConflict = handledResult.IsConcurrencyConflict;
        bool isUniqueConstraintViolation = handledResult.IsUniqueConstraintViolation;
        bool isForeignKeyConstraintViolation = handledResult.IsForeignKeyConstraintViolation;
        if (isConcurrencyConflict || isUniqueConstraintViolation || isForeignKeyConstraintViolation)
        {
            throw new ConcurrencyException();
        }
    }
    #endregion

    #region PrivateStaticMethods
    private static void MapItem(SupplyUpsertItemRequestDto itemRequestDto, SupplyItem item)
    {
        item.AmountPerUnit = itemRequestDto.AmountPerUnit;
    }
    #endregion
}