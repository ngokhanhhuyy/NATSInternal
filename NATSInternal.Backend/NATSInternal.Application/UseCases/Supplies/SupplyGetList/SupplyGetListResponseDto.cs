using NATSInternal.Application.Authorization;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Supplies;

namespace NATSInternal.Application.UseCases.Supplies;

public class SupplyGetListResponseDto : IListResponseDto<SupplyGetListSupplyResponseDto>
{
    #region Constructors
    public SupplyGetListResponseDto(
        IEnumerable<SupplyGetListSupplyResponseDto> responseDtos,
        int pageCount,
        int itemCount)
    {
        Items = responseDtos;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public IEnumerable<SupplyGetListSupplyResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}

public class SupplyGetListSupplyResponseDto
{
    #region Constructors
    internal SupplyGetListSupplyResponseDto(
        Supply supply,
        long itemAmount,
        Photo? thumbnail,
        SupplyExistingAuthorizationResponseDto authorization)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        ItemAmount = itemAmount;
        TransactionDateTime = supply.TransactionDateTime;
        Thumbnail = thumbnail is not null ? new(thumbnail) : null;
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public long ShipmentFee { get; }
    public long ItemAmount { get; }
    public DateTime TransactionDateTime { get; }
    public PhotoBasicResponseDto? Thumbnail { get; }
    public SupplyExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}