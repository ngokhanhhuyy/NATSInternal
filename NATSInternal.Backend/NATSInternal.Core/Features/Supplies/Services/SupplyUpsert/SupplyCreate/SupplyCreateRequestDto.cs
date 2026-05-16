namespace NATSInternal.Core.Features.Supplies;

public class SupplyCreateRequestDto : AbstractSupplyUpsertRequestDto
{
    #region Properties
    public bool IsCorrection { get; set; }
    #endregion
}
