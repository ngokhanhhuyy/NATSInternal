using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Orders;

public class OrderListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = false;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.StatsDate);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public List<int> CustomerIds { get; set; } = new();
    public bool DebtOrdersOnly { get; set; }
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