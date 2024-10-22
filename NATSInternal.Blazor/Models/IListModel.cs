namespace NATSInternal.Blazor.Models;

public interface IListModel<TModel>
{
    string OrderByField { get; set; }
    bool OrderByAscending { get; set; }
    int Page { get; set; }
    int ResultsPerPage { get; set; }
    int PageCount { get; set; }
    List<TModel> Items { get; set; }
    PaginationRangeModel PaginationRanges { get; }
}