using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Supplies;

public class SupplyListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.StatsDate);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public int? StatsYear { get; set; }
    public int? StatsMonth { get; set; }
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
        ItemAmount,
        TotalAmount
    }
    #endregion
}
