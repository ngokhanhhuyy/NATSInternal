using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Debts;

public abstract class DebtUpsertRequestDto : IHasStatsUpsertRequestDto
{
    #region Properties
    public DateOnly? StatsDate { get; set; }
    public long Amount { get; set; }
    public string? Note { get; set; }
    #endregion

    #region Methods
    public virtual void TransformValues()
    {
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}
