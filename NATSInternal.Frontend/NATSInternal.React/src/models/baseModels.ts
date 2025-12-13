declare global {
  interface ISortableListModel<TItemModel extends object> {
    sortByAscending: boolean;
    sortByFieldName: string;
    items: TItemModel[];
  }

  interface IPageableListModel<TItemModel extends object> {
    page: number;
    resultsPerPage: number;
    items: TItemModel[];
    pageCount: number;
  }

  interface ISortableAndPageableListModel<TItemModel extends object> extends
      ISortableListModel<TItemModel>,
      IPageableListModel<TItemModel> {
    createRoute: string;
  }

  interface ISearchablePagableListModel<TItemModel extends object> extends ISortableAndPageableListModel<TItemModel> {
    searchContent: string;
  }
}