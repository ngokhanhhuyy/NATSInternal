namespace NATSInternal.Web.Models;

public interface IListModel
{
    #region Properties
    bool? SortByAscending { get; set; }
    string? SortByFieldName { get; set; }
    int? Page { get; set; }
    int? ResultsPerPage { get; set; }
    int PageCount { get; }
    int ItemsCount { get; }
    IReadOnlyList<string> SortByFieldNameOptions { get; }
    #endregion
}

public interface IListModel<out TItemModel> : IListModel
{
    #region Properties
    IReadOnlyList<TItemModel> Items { get; }
    #endregion
}