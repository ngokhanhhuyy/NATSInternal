using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Orders;

public class OrderListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = false;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.StatsDate);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public ListMonthYearRequestDto? StatsMonthYear { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        StatsDate,
        CreatedDateTime,
        LastUpdatedDateTime,
        ProductItemsAmount,
        ServiceItemsAmount,
        TotalAmount
    }
    #endregion
}