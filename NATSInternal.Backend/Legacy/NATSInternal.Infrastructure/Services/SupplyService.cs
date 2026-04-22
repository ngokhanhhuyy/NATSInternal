using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Supplies;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Supplies;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Extensions;

namespace NATSInternal.Infrastructure.Services;

[UsedImplicitly]
internal class SupplyService : ISupplyService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion

    #region Constructors
    public SupplyService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationInternalService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationInternalService = authorizationInternalService;
    }
    #endregion

    #region Methods
    public async Task<SupplyGetListResponseDto> GetPaginatedSupplyListAsync(
        SupplyGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Supply> query = _context.Supplies.Where(p => p.DeletedDateTime == null && p.DeletedUserId == null);

        if (requestDto.TransactionRangeStartingDateTime.HasValue)
        {
            DateTime transactionRangeStartingDateTime = requestDto.TransactionRangeStartingDateTime.Value;
            query = query.Where(p => p.TransactionDateTime >= transactionRangeStartingDateTime);
        }
        
        if (requestDto.TransactionRangeEndingDateTime.HasValue)
        {
            DateTime transactionRangeStartingDateTime = requestDto.TransactionRangeEndingDateTime.Value;
            query = query.Where(p => p.TransactionDateTime >= transactionRangeStartingDateTime);
        }

        var projectedQuery = query.Select(supply => new SupplyWithItemAmountAndThumbnail
        {
            Supply = supply,
            ItemAmount = EF.Property<long>(supply, "CachedItemAmount"),
            Thumbnail = _context.Photos.SingleOrDefault(p => p.SupplyId == supply.Id && p.IsThumbnail)
        });

        switch (requestDto.SortByFieldName)
        {
            case nameof(SupplyGetListRequestDto.FieldToSort.CreatedDateTime):
                projectedQuery = projectedQuery.ApplySorting(
                    siat => siat.Supply.CreatedDateTime,
                    requestDto.SortByAscending
                );
                break;
            case nameof(SupplyGetListRequestDto.FieldToSort.ItemAmount):
                projectedQuery = projectedQuery.ApplySorting(
                    siat => siat.ItemAmount,
                    requestDto.SortByAscending
                );
                break;
            case nameof(SupplyGetListRequestDto.FieldToSort.TotalAmount):
                projectedQuery = projectedQuery.ApplySorting(
                    siat => siat.Supply.ShipmentFee + siat.ItemAmount,
                    requestDto.SortByAscending
                );
                break;
            default:
                throw new NotImplementedException();
        }

        Page<SupplyWithItemAmountAndThumbnail> queryResult = await _listFetchingService.GetPagedListAsync(
            projectedQuery,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<SupplyGetListSupplyResponseDto> productResponseDtos = queryResult.Items
            .Select(siat => new SupplyGetListSupplyResponseDto(
                siat.Supply,
                siat.ItemAmount,
                siat.Thumbnail,
                _authorizationInternalService.GetSupplyExistingAuthorization(siat.Supply)))
            .ToList();

        return new(productResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }
    #endregion
}

file class SupplyWithItemAmountAndThumbnail
{
    #region Properties
    public required Supply Supply { get; init; }
    public required long ItemAmount { get; init; }
    public required Photo? Thumbnail { get; init; }
    #endregion
}