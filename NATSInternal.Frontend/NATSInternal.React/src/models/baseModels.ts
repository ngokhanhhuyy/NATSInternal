declare global {
  interface IListModel<TItemModel extends object> {
    sortByAscending: boolean;
    sortByFieldName: string;
    sortByFieldNameOptions: string[];
    page: number;
    resultsPerPage: number;
    items: TItemModel[];
    pageCount: number;
    itemCount: number;
  }

  interface ISearchableListModel<TItemModel extends object> extends IListModel<TItemModel> {
    searchContent: string;
    items: TItemModel[];
  }

  interface IUpsertableListModel<TItemModel extends object> {
    items: TItemModel[];
    createRoutePath?: string;
  }
}

export { };
