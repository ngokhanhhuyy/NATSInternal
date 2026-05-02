using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyBasicResponseDto
{
    #region Constructors
    internal SupplyBasicResponseDto(Supply supply)
    {
        Id = supply.Id;
        ShipmentFee = supply.ShipmentFee;
        ItemAmount = supply.CachedItemsAmount;
        StatsDateTime = supply.StatsDateTime;

        if (supply.Thumbnail is not null)
        {
            Thumbnail = new(supply.Thumbnail);
        }
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
    public DateTime StatsDateTime { get; }
    public PhotoBasicResponseDto? Thumbnail { get; }
    public SupplyExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}