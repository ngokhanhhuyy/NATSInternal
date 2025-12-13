declare global {
  interface IPageableListModel<TItemModel extends object> {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number | null;
    items: TItemModel[];
    pageCount: number;
    get sortByFieldNameOptions(): string[];
    get createRoute(): string;
  }
  
  interface ISearchablePagableListModel<TItemModel extends object> extends IPageableListModel<TItemModel> {
    searchContent: string;
  }
}