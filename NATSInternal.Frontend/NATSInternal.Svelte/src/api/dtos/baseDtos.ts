declare global {
  interface ISortableListRequestDto {
    sortByAscending: boolean;
    sortByFieldName: string;
  }

  interface IPageableListRequestDto {
    page: number;
    resultsPerPage: number;
  }

  interface ISearchableListRequestDto {
    searchContent: string;
  }

  interface IPageableListResponseDto<TItem extends object> {
    items: TItem[];
    pageCount: number;
    itemCount: number;
  }

  interface IUpsertableListResponseDto<TItem extends IUpsertableExistingAuthorizationResponseDto>
    extends IPageableListResponseDto<TItem> { }

  interface IUpsertableExistingAuthorizationResponseDto {
    canEdit: boolean;
    canDelete: boolean;
  }
}

export { };