using NATSInternal.Application.Authorization;
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
    internal SupplyGetListSupplyResponseDto(Supply supply, SupplyExistingAuthorizationResponseDto authorization)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        ItemAmount = supply.Items.Sum(i => i.Amount);
        CreatedDateTime = supply.CreatedDateTime;
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public long ShipmentFee { get; }
    public long ItemAmount { get; }
    public DateTime CreatedDateTime { get; }
    public SupplyExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}