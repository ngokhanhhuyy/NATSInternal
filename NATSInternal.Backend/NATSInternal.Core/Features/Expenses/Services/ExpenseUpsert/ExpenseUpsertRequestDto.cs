using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Expenses;

public class ExpenseUpsertRequestDto : IHasStatsUpsertRequestDto
{
    #region Properties
    public DateOnly? StatsDate { get; set; }
    public long Amount { get; set; }
    public ExpenseType Type { get; set; }
    public string? Note { get; set; }
    public List<PhotoUpsertRequestDto> Photos { get; set; } = new();
    #endregion

    #region Methods
    public void TransformValues()
    {
        Note = Note?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion
}