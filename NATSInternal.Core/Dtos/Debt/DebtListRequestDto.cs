namespace NATSInternal.Core.Dtos;

public class DebtListRequestDto
    :
        IHasStatsListRequestDto,
        IHasCustomerListRequestDto
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public ListMonthYearRequestDto? MonthYear { get; set; }
    public Guid? CustomerId  { get; set; }
    public Guid? CreatedUserId { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SortByFieldName = SortByFieldName?.ToNullIfEmptyOrWhiteSpace();

        if (CreatedUserId == Guid.Empty)
        {
            CreatedUserId = null;
        }

        if (CustomerId ==  Guid.Empty)
        {
            CustomerId = null;
        }
        
        if (CustomerId ==  Guid.Empty)
        {
            CustomerId = null;
        }

        if (CreatedUserId ==  Guid.Empty)
        {
            CreatedUserId = null;
        }
    }
    #endregion
}