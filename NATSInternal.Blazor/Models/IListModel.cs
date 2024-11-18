namespace NATSInternal.Blazor.Models;

public interface IListModel<TModel>
{
    string SortingByField { get; set; }
    bool SortingByAscending { get; set; }
    int Page { get; set; }
    int ResultsPerPage { get; set; }
    int PageCount { get; set; }
    List<TModel> Items { get; set; }
}