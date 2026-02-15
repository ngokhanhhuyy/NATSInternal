declare global {
  interface ISortableListModel<TItemModel extends object> {
    sortByAscending: boolean;
    sortByFieldName: string;
    items: TItemModel[];
    sortByFieldNameOptions: string[];
  }

  interface IPageableListModel<TItemModel extends object> {
    page: number;
    resultsPerPage: number;
    items: TItemModel[];
    pageCount: number;
    itemCount: number;
  }

  interface ISearchableListModel<TItemModel extends object> {
    searchContent: string;
    items: TItemModel[];
  }

  interface IUpsertableListModel<TItemModel extends object> {
    items: TItemModel[];
    createRoutePath: string;
  }
}

export { };