using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyBasicResponseDto
{
    #region Constructors
    internal SupplyBasicResponseDto(Supply supply)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        ItemAmount = supply.CachedItemsAmount;
        StatsDate = supply.StatsDate;
        ThumbnailUrl = supply.Thumbnail?.Url;
    }

    internal SupplyBasicResponseDto(Supply supply, SupplyExistingAuthorizationResponseDto authorization) : this(supply)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public long ShipmentFee { get; }
    public long ItemAmount { get; }
    public DateOnly StatsDate { get; }
    public string? ThumbnailUrl { get; }
    public SupplyExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}
