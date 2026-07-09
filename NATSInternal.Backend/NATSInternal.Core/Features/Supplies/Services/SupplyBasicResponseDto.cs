using NATSInternal.Core.Features.Authorization;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyBasicResponseDto
{
    #region Constructors
    internal SupplyBasicResponseDto(Supply supply)
    {
        Id = supply.Id;
        Amount = supply.CachedAmount;
        StatsDate = supply.StatsDate;
        ThumbnailUrl = supply.Thumbnail?.Url;

        if (supply.Items.Count > 0)
        {
            ProductCount = supply.Items.Count;
        }
    }

    internal SupplyBasicResponseDto(Supply supply, SupplyExistingAuthorizationResponseDto authorization) : this(supply)
    {
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public long Amount { get; }
    public int? ProductCount { get; }
    public DateOnly StatsDate { get; }
    public string? ThumbnailUrl { get; }
    public SupplyExistingAuthorizationResponseDto? Authorization { get; }
    #endregion
}
