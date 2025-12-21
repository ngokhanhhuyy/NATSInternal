using NATSInternal.Application.Localization;

namespace NATSInternal.Web.Models;

public interface IPageableListModel<out TItemModel>
{
    #region Properties
    bool? SortByAscending { get; set; }
    string? SortByFieldName { get; set; }
    int? Page { get; set; }
    int? ResultsPerPage { get; set; }
    IReadOnlyList<TItemModel> Items { get; }
    int PageCount { get; }
    int ItemsCount { get; }
    IReadOnlyList<string> SortByFieldNameOptions { get; }
    #endregion
}