namespace NATSInternal.Web.Models;

public interface ISearchableListModel : IListModel
{
    #region Properties
    string? SearchContent { get; set; }
    #endregion
}

public interface ISearchableListModel<out TItemModel> : IListModel<TItemModel>, ISearchableListModel;