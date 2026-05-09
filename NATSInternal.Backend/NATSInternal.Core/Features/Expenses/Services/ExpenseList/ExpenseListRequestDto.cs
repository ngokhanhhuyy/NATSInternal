using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Expenses;

public class ExpenseListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.StatsDate);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public ListMonthYearRequestDto? StatsMonthYear { get; set; }
    public string? SearchContent { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SearchContent = SearchContent.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        StatsDate,
        CreatedDateTime,
        LastUpdatedDateTime,
        Amount
    }
    #endregion
}