using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyUpsertRequestDto : IHasProductUpsertRequestDto<SupplyUpsertItemRequestDto>
{
    #region Properties
    public List<SupplyUpsertItemRequestDto> Items { get; set; } = new();
    public DateTime? StatsDateTime { get; set; }
    public long ShipmentFee { get; set; }
    public string? Note { get; set; }
    public List<PhotoUpsertRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Note = Note.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}